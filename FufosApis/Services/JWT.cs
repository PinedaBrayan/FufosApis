
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FufosApis.Services;
using FufosEntities.DTOS;
using FufosEntities.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FufosApis.Services
{
    public interface IJWTService
    {
        public string Generate(User obj);

        public T Validate<T>(string token);
    }

    public class JWTService(IOptions<TokenConfiguration> options) : IJWTService
    {
        // readonly string Salt;
        readonly string SecretKey = options.Value.Secret;
        readonly int MinutesExp = options.Value.MinutesExp;

        public string Generate(User? obj)
        {
            if(obj is null)
                return string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();

            var JwtUser = new JWTUserDTO()
            {
                Rowid = obj.Rowid,
                Email = obj.Email,
                FullName = obj.FullName,
                IsAdmin = obj.IsAdmin,
                IsEmployee = obj.IsEmployee
            };

            var key = Encoding.ASCII.GetBytes(SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("user", JsonConvert.SerializeObject(JwtUser, Formatting.None))
                }),
                Expires = DateTime.UtcNow.AddMinutes(MinutesExp),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
                )
            };

            var Value = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(Value);
        }

        public T Validate<T>(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var user = jwtToken.Claims.First(x => x.Type == "user");

            var Result = JsonConvert.DeserializeObject<T>(user.Value)!;

            return Result;
        }
    }
}