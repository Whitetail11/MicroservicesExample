using MicroservicesExample.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace MicroservicesExample.Auth.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private ICollection<User> _users = new List<User>();
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _users.Add(new User() { Id = new Guid(), UserName = "Test", Password = "12345" });
        }

        public async Task<string> SignIn(User model)
        {
            var user = _users.FirstOrDefault(u => u.UserName == model.UserName);
            if (user != null && model.Password == user.Password)
            {
                return await GenerateJwtToken(model);
            }
            else
            {
                throw new Exception("incorrect data");
            }
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow + TimeSpan.ParseExact(_configuration["JWT:Expire"], "c", null),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return tokenHandler.WriteToken(token);
        }
    }
}
