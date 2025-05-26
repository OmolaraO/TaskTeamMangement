  using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamTaskManagement.DTOs;
using TeamTaskManagement.Services.Interfaces;

namespace TeamTaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : BaseController
    {
        

        [HttpPost("register")]
        public async Task <IActionResult> Register([FromBody] RegisterDto registerDto, CancellationToken cancellationToken)
        {
            
                var result = await authService.RegisterAsync(registerDto, cancellationToken);
                return StatusCode(result.StatusCode, result);
            
        }

        [HttpPost("login")]
        public async Task <IActionResult> Login([FromBody] LoginDto loginRequest, CancellationToken cancellationToken)
        {
          
                var result = await authService.LoginAsync(loginRequest, cancellationToken);
                return StatusCode(result.StatusCode, result);
            
            
        }
    }
}
