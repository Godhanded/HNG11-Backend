using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserOrgs.Dto;

namespace UserOrgs.Services
{
    public class TokenService(IConfiguration config)
    {
        private readonly IConfiguration _config = config;

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration _config) => new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey=GetSecurityKey(_config),
        };

        public string GenerateJwt(UserDataDto userData)
        {
            SymmetricSecurityKey securityKey = GetSecurityKey(_config);

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = [
                new(ClaimTypes.NameIdentifier,userData.userId),
                new(ClaimTypes.Email,userData.email),
                new(ClaimTypes.Name,userData.firstName)
                ];

            var expireInMinutes = Convert.ToInt32(_config["Jwt:ExpireInMinute"]);
            var tokenObject = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddMinutes(expireInMinutes)
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenObject);
        }

        private static SymmetricSecurityKey GetSecurityKey(IConfiguration _config)
        {
            var secretKey = _config["Jwt:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            return securityKey;
        }
    }
}
