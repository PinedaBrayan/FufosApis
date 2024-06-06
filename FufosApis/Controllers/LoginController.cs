using Appointment.SDK.Backend.Controllers;
using FufosEntities.DTOS;
using Microsoft.AspNetCore.Mvc;
using FufosEntities.Entities;
using FufosEntities.Utilities;
using System.ComponentModel.DataAnnotations;
using FufosApis.Utilities;
using FufosApis.Services;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace FufosApis.Controllers
{
    [ApiController]
    [Route("/api")]
    public class LoginController(IServiceProvider serviceProvider, IOptions<TokenConfiguration> options, IJWTService jwtService) 
        : StandardController(serviceProvider)
    {
        readonly IJWTService _jWTService = jwtService;
        readonly ITokenConfiguration _tokenConfiguration = options.Value;

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO User)
        {
            using(var context = CreateContext())
            {
                var UserBd = context.Set<User>()
                    .AsNoTracking()
                    .Where(x => x.Email == User.Email)
                    .Select(x => new User{
                        Rowid = x.Rowid,
                        FullName = x.FullName,
                        Email = x.Email,
                        Password = x.Password,
                        IsAdmin = x.IsAdmin,
                        IsEmployee = x.IsEmployee
                    }).FirstOrDefault();

                if(UserBd is null)
                    return BadRequest(new { Errors = "Invalid credentials" });

                var PasswordIsValid = Utils.Compare(_tokenConfiguration.Salt, User.Password, UserBd.Password);

                if(!PasswordIsValid)
                    return BadRequest(new { Errors = "Invalid credentials" });

                var Token = _jWTService.Generate(UserBd);
                return Ok(new{ Data = Token });
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserDTO User)
        {
            var NewUser = new User()
            {
                FullName = User.FullName ?? $"{Guid.NewGuid()}",
                Email = User.Email,
                Password = User.Password
            };

            var Errors = new List<ValidationResult>();

            var IsValid = DataAnnotationsValidator.Validate<User>(NewUser, ref Errors);

            if (!IsValid)
                return BadRequest(new {Errors = Errors.Select(x => x.ErrorMessage)});

            NewUser.FullName = User.FullName ?? User.Email.Split("@")[0];

            NewUser.Password = Utils.HashTo256(NewUser.Password, _tokenConfiguration.Salt);

            using (var dbContext = CreateContext())
            {
                dbContext.Add(NewUser);
                dbContext.SaveChanges();
            }

            var Token = _jWTService.Generate(NewUser);
            return Ok(new{ Data = Token });
        }
    }
}
