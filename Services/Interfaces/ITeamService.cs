using TeamTaskManagement.DTOs;
using TeamTaskManagement.Models;

namespace TeamTaskManagement.Services.Interfaces
{
    public interface ITeamService
    {
        Task<GenericResponse<TeamDto>> CreateTeamAsync(long userId, CreateTeamDto createTeamDto);
        Task<GenericResponse<string>> AddUserToTeamAsync(int teamId, long currentUserId, AddUserToTeamDto addUserToTeamDto);
        
        Task<GenericResponse<IEnumerable<TaskDto>>> GetTeamTasksAsync(int teamId, long userId);
        
        Task<GenericResponse<TaskDto>> CreateTaskAsync(int teamId, long currentUserId, CreateTaskDto dto);
    }
}
