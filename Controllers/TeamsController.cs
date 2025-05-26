using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TeamTaskManagement.DTOs;
using TeamTaskManagement.Services.Interfaces;

namespace TeamTaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamsController(ITeamService teamService) : BaseController
    {
        

        [HttpPost]
        public async Task <IActionResult> CreateTeamAsync(int teamId, [FromBody] CreateTeamDto dto)
        {
            var currentUserId = long.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            var result = await teamService.CreateTeamAsync(currentUserId, dto);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("{teamId}/users")]
        public async Task<IActionResult> AddUserToTeamAsync(int teamId, [FromBody] AddUserToTeamDto dto)
        {
            var currentUserId = long.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            var result = await teamService.AddUserToTeamAsync(teamId, currentUserId, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{teamId}/tasks")]
        public async Task<IActionResult> GetTeamTasksAsync(int teamId)
        {
            var currentUserId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            var result = await teamService.GetTeamTasksAsync(teamId, currentUserId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{teamId}/tasks")]
        public async Task<IActionResult> CreateTask(int teamId, [FromBody] CreateTaskDto createTaskDto)
        {
            var currentUserId = int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
            var result = await teamService.CreateTaskAsync(teamId, currentUserId, createTaskDto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
