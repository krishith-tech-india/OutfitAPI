using Core.AppSettingConfigs;
using Data.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Authentication
{
    public static class UserAuthentication
    {
        public static string GenerateTokenAsync(this User user, JWTConfigrations jwt)
        {
            //9.
            var claims = new[]
            {
            new Claim (JwtRegisteredClaimNames.Sub, jwt.Subject),
            new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("id", user.Id.ToString()),
            new Claim("phone", user.PhNo),
            new Claim("email", user.Email)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
