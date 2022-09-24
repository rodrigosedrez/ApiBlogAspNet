using ApiBlog.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using ApiBlogAspNet.Extensions;

namespace ApiBlogAspNet.Services
{
    public class TokenService
    {
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
            var claims = user.GetClaims();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                    
                //    new Claim[]       obs: this code direct injet user on the com and the claims use roleclaimsExtension
                //{
                //    new Claim(type:ClaimTypes.Name, value: "rodrigosedrez"), //User.Identity.Name
                //    new Claim(type:ClaimTypes.Role, value: "user"), // User.IsInRole
                //    new Claim(type:ClaimTypes.Role, value: "admin")
                //     //new Claim("menage", value:"queremos")
                //}),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
