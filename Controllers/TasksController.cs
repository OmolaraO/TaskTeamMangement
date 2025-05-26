using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TeamTaskManagement.Data;
using TeamTaskManagement.DTOs;
using TeamTaskManagement.Services.Interfaces;

namespace TeamTaskManagement.Controllers
{
    [Route("teams")]
    [ApiController]
    [Authorize]
    public class TasksController : BaseController
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskDto updateTaskDto)
        {
            var currentUserId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            var task = await _context.Tasks
                .Include(t => t.Team)
                .ThenInclude(t => t.TeamUsers)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) return NotFound();
            if (!task.Team.TeamUsers.Any(tu => tu.UserId == currentUserId)) return Forbid();

            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.DueDate = updateTaskDto.DueDate;
            task.AssignedToUserId = updateTaskDto.AssignedToUserId;

            await _context.SaveChangesAsync();
            return Ok("Task updated.");
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var currentUserId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            var task = await _context.Tasks
                .Include(t => t.Team)
                .ThenInclude(t => t.TeamUsers)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) return NotFound();
            if (!task.Team.TeamUsers.Any(tu => tu.UserId == currentUserId)) return Forbid();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok("Task deleted.");
        }

        [HttpPatch("{taskId}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int taskId,
            [FromBody] UpdateTaskStatusDto updateTaskStatusDto)
        {
            var currentUserId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            var task = await _context.Tasks
                .Include(t => t.Team)
                .ThenInclude(t => t.TeamUsers)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) return NotFound();
            if (!task.Team.TeamUsers.Any(tu => tu.UserId == currentUserId)) return Forbid();

            task.Status = updateTaskStatusDto.Status;
            await _context.SaveChangesAsync();
            return Ok("Task status updated.");
        }


        [HttpGet("teams/{teamId}/tasks")]
        public async Task<IActionResult> GetTask(int taskId)
        {
            var currentUserId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            var task = await _context.Tasks
                .Include(t => t.Team)
                .ThenInclude(t => t.TeamUsers)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) return NotFound();
            if (!task.Team.TeamUsers.Any(tu => tu.UserId == currentUserId)) return Forbid();

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

            return Ok(taskDto);
        }
    }
}