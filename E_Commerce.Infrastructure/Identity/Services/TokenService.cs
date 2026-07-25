using E_Commerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTSettings _jwtSettings;
        public TokenService(IOptions<JWTSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }
        public string CreateToken(string userId, string email, string userName, IReadOnlyList<string> roles)
        {
            // claims [payload]
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, userName)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // type , security algo
            // appsetting
            var secKey = _jwtSettings.SecretKey;
            if (string.IsNullOrWhiteSpace(secKey))
                throw new InvalidOperationException("Secret Key Is Empty !");
            if (secKey.Length < 20)
                throw new InvalidOperationException("Secret Key Too Short");
            // byte[]  
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secKey));
            // sign the token
            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            // Jwt token  bearer
            var token = new JwtSecurityToken
                (
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


    public class JWTSettings
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int ExpirationMinutes { get; set; }
    }

}
