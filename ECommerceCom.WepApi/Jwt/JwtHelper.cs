using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceCom.WepApi.Jwt
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(JwtDto jwtInfo)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SecretKey));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtClaimNames.Id, jwtInfo.Id.ToString()),
                new Claim(JwtClaimNames.Email, jwtInfo.Email),
                new Claim(JwtClaimNames.Name, jwtInfo.Name),
                new Claim(JwtClaimNames.LastName, jwtInfo.LastName),
                new Claim(JwtClaimNames.UserRole, jwtInfo.Role.ToString()),
                new Claim(ClaimTypes.Role, jwtInfo.Role.ToString()),
            };
            var expireTime = DateTime.Now.AddMinutes(jwtInfo.ExpireMinutes);
            var tokenDescriptor = new JwtSecurityToken(jwtInfo.Issuer, jwtInfo.Audience, claims, null, expireTime, credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }
    }
}
