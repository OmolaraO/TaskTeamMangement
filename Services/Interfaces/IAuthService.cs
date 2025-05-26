using Microsoft.AspNetCore.Identity.Data;
using TeamTaskManagement.DTOs;

namespace TeamTaskManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<GenericResponse<string>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken);
        Task<GenericResponse<string>> LoginAsync(LoginDto login, CancellationToken cancellationToken);
    }
}
