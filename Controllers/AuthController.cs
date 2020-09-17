using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SocialSite.API.Data;
using SocialSite.API.Dto;
using SocialSite.API.Models;
using SocialSite.API.Helpers;

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

            if (userToRegister.Password != userToRegister.RePassword)
            {
                return BadRequest("Passwords do not match!");
            }

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
                ProfileImagePath = "placeholder.jpg"
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
            JwtHandlerCreator tokenCreator = new JwtHandlerCreator(this.config);

            var token = tokenCreator.CreateToken(tokenHandler, user); 

            return Ok(new 
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}