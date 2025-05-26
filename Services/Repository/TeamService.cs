using Microsoft.EntityFrameworkCore;
using TeamTaskManagement.Data;
using TeamTaskManagement.DTOs;
using TeamTaskManagement.Models;
using TeamTaskManagement.Services.Interfaces;
using TaskStatus = TeamTaskManagement.Models.TaskStatus;

namespace TeamTaskManagement.Services.Repository
{
    public class TeamService(AppDbContext context) : ITeamService
    {


        public async Task<GenericResponse<TeamDto>> CreateTeamAsync(long userId, CreateTeamDto createTeamDto)
        {
            var team = new Team
            {
                Name = createTeamDto.Name,
                Description = createTeamDto.Description
            };
            
            await context.Teams.AddAsync(team);
            await context.SaveChangesAsync();
            
            await context.TeamUsers.AddAsync(new TeamUser { TeamId = team.Id, UserId = userId });
            await context.SaveChangesAsync();
            
            var teamDto = new TeamDto { Id = team.Id, Name = team.Name, Description = team.Description };
            
            return GenericResponse<TeamDto>.Success("Success", teamDto);
            
        }

        public async Task<GenericResponse<string>> AddUserToTeamAsync(int teamId, long currentUserId,
            AddUserToTeamDto addUserToTeamDto)
        {
            var isMember = await context.TeamUsers.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == currentUserId);
            if (!isMember)
            {
                return GenericResponse<string>.Error(403, "You are not a member of this team.");
            }
            
            if (!await context.Users.AnyAsync(u => u.Id == addUserToTeamDto.UserId))
            {
                return GenericResponse<string>.Error(404, "User not found.");
            }

            if (await context.TeamUsers.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == addUserToTeamDto.UserId))
            {
                return GenericResponse<string>.Error(409, "User is already a team owner.");
            }
            
            context.TeamUsers.Add(new TeamUser { TeamId = teamId, UserId = addUserToTeamDto.UserId });
            await context.SaveChangesAsync();
            
            return GenericResponse<string>.Success("Success", "");
               

        }

        public async Task<GenericResponse<IEnumerable<TaskDto>>> GetTeamTasksAsync(int teamId, long userId)
        {
            
            var isMember = await context.TeamUsers.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == userId);
            if (!isMember)
            {
                return GenericResponse<IEnumerable<TaskDto>>.Error(403, "You are not a member of this team.");
            }

            var tasks = await context.Tasks.AsNoTracking()
                .Where(t => t.TeamId == teamId).ToListAsync();

            var tasksDto = tasks.Select(t => new TaskDto()
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                CreatedAt = t.CreatedAt,
                AssignedToUserId = t.AssignedToUserId,
                CreatedByUserId = t.CreatedByUserId,
                TeamId = t.TeamId
            });
            
            return GenericResponse<IEnumerable<TaskDto>>.Success("Success", tasksDto);
        }

        public async Task<GenericResponse<TaskDto>> CreateTaskAsync(int teamId, long currentUserId, CreateTaskDto dto)
        {
            var isMember = await context.TeamUsers.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == currentUserId);
            if (!isMember)
            {
                return GenericResponse<TaskDto>.Error(403, "You are not a member of this team.");
            }

            var assignedUserIsMember = await context.TeamUsers.AnyAsync(tu => tu.TeamId == teamId && tu.UserId == dto.AssignedToUserId);
            if (!assignedUserIsMember)
            {
                return GenericResponse<TaskDto>.Error(401, "Assigned user is not a member of the team.");
            }

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Status = TaskStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                AssignedToUserId = dto.AssignedToUserId,
                CreatedByUserId = currentUserId,
                TeamId = teamId
            };

            await context.Tasks.AddAsync(task);
            await context.SaveChangesAsync();

            var taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Status = task.Status,
                CreatedAt = task.CreatedAt,
                AssignedToUserId = task.AssignedToUserId,
                CreatedByUserId = task.CreatedByUserId,
                TeamId = task.TeamId
            };
            
            return GenericResponse<TaskDto>.Success("Success", taskDto);
        }
    }
}
