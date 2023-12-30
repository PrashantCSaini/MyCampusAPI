using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyCampus
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {

        private readonly string key;
        public JwtAuthenticationManager(string key)
        {
            this.key = key;
        }

        public string Authenticate(string username, int id, char type)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.SerialNumber, id.ToString()),
                    new Claim(ClaimTypes.Role, type.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials =
            new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
