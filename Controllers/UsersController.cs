using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TeamTaskManagement.Data;
using TeamTaskManagement.DTOs;
using TeamTaskManagement.Services.Interfaces;
using TeamTaskManagement.Services.Mappings;

namespace TeamTaskManagement.Controllers
{
    [Route("users")]
    [ApiController]
    [Authorize]
    public class UsersController(AppDbContext context) : BaseController
    {
        
        [HttpGet("me")]
        [Authorize]
        public async Task <IActionResult> GetMe()
        {
            _ = long.TryParse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value, out long userId);
            var result = await GetUserInfo(userId);
            
            return StatusCode(result.StatusCode, result);
        }

        private async Task<GenericResponse<UserDto>> GetUserInfo(long userId)
        {
            var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
            
            return GenericResponse<UserDto>.Success("Success", user.MapToDto());
        }
    }
}
