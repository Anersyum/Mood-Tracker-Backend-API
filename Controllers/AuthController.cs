using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialSite.API.Data;
using SocialSite.API.Dto;
using SocialSite.API.Models;

namespace SocialSite.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepo;
        private readonly IConfiguration config;

        public AuthController(IAuthRepository authRepo, IConfiguration config)
        {
            this.config = config;
            this.authRepo = authRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisteredUserDto userToRegister)
        {
            string usernameToLower = userToRegister.Username.ToLower();
            string emailToLower = userToRegister.Email.ToLower();

            if (!this.authRepo.ValidateEmail(emailToLower))
            {
                return BadRequest("You haven't entered a valid email.");
            }

            if (await this.authRepo.UserExits(usernameToLower, emailToLower))
            {
                return BadRequest("Username exits!");
            }

            User user = new User() {
                Username = usernameToLower,
                Email = emailToLower,
                FirstName = userToRegister.FirstName,
                LastName = userToRegister.LastName,
                DateOfBirth = userToRegister.DateOfBirth
            };

            user = await this.authRepo.Register(user, userToRegister.Password);

            return StatusCode(201, user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto userToLogin)
        {
            string usernameToLower = userToLogin.Username.ToLower();

            User user = await this.authRepo.Login(usernameToLower, userToLogin.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            user.Username = user.Username.ToLower();
            
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var token = CreateToken(tokenHandler, user); 

            return Ok(new 
            {
                token = tokenHandler.WriteToken(token)
            });
        }

        private SecurityToken CreateToken(JwtSecurityTokenHandler tokenHandler, User user)
        {
            // we build the token with claims
            var claims = new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            // here we sign the token so that we can authenticate the token on the server side
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // creating the token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}