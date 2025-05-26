using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TeamTaskManagement.Data;
using TeamTaskManagement.DTOs;
using TeamTaskManagement.Models;
using TeamTaskManagement.Services.Interfaces;
using TeamTaskManagement.Services.Mappings;

namespace TeamTaskManagement.Services.Repository
{
    public class AuthService(AppDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<GenericResponse<string>> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken)
        {
            var existingUser = await context
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => 
                    x.Username.Trim().ToUpper() == registerDto.Username.ToUpper().Trim(), cancellationToken);

            if (existingUser is not null)
            {
                return GenericResponse<string>.Error(409, "Username is taken");
            }

            var userToBeSaved = registerDto.MapToEntity();
            
            await context.Users.AddAsync(userToBeSaved, cancellationToken);
            
            await context.SaveChangesAsync(cancellationToken);
            
            return GenericResponse<string>.Success("Success", "");


        }

        public async Task<GenericResponse<string>> LoginAsync(LoginDto login, CancellationToken cancellationToken)
        {
            var user = await context
                    .Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => 
                        x.Username.Trim().ToUpper() == login.Username.Trim().ToUpper(), cancellationToken);

            if (user is null || !BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                return GenericResponse<string>.Error(404, "Invalid username or password.");
            }
            
            var token = GenerateJwtToken(user);
            
            return GenericResponse<string>.Success("Success", token);
            
            

            
            
            
        }
        
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
    
}
