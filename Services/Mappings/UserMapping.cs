using TeamTaskManagement.DTOs;
using TeamTaskManagement.Models;

namespace TeamTaskManagement.Services.Mappings;

public static class UserMapping
{
    public static User MapToEntity(this RegisterDto dto)
    {
        return new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
    }

    public static UserDto MapToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
        };
    }
}