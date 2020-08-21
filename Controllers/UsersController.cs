using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SocialSite.API.Data;
using SocialSite.API.Dto;
using SocialSite.API.Helpers;
using SocialSite.API.Models;

namespace SocialSite.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepo;
        private readonly IConfiguration config;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepo, IConfiguration config, IMapper mapper)
        {
            this.mapper = mapper;
            this.config = config;
            this.userRepo = userRepo;
        }

        [HttpGet("get/{username}")]
        public async Task<IActionResult> GetUsersByUsernameAction(string username)
        {
            var users = await this.userRepo.GetUsersByUsername(username);
            var mappedUsers = this.mapper.Map<IEnumerable<UserToListDto>>(users);
            
            return Ok(mappedUsers);
        }

        [HttpGet("get/user/{id}")]
        public async Task<IActionResult> GetUserByIdAction(int id)
        {
            var userToReturn = await this.userRepo.GetUserViaId(id);

            DetailedUserDto user = this.mapper.Map<DetailedUserDto>(userToReturn);
            
            user.DateOfBirth = userToReturn.DateOfBirth.ToShortDateString();
            return Ok(user);
        }
        
        [HttpPost("edit/user")]
        public async Task<IActionResult> EditUser([FromForm]EditUserDto userToEdit)
        {
            if (userToEdit == null)
            {
                return BadRequest("Invalid request.");
            }

            if (userToEdit.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            User user = await this.userRepo.EditUser(userToEdit);

            if (user == null)
            {
                return BadRequest("User could not be edited");
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtHandlerCreator tokenCreator = new JwtHandlerCreator(this.config);

            var token = tokenCreator.CreateToken(tokenHandler, user);

            return Ok(new 
            {
                success = true,
                message = "Edited user.",
                token = tokenHandler.WriteToken(token)
            });
        }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteUser(UserDto user)
        {
            return Ok("User deleted.");
        }

        [AllowAnonymous]
        [HttpGet("user/{id}/{url}")]
        public async Task<IActionResult> GetImage(int id, string url)
        {
            string type = url.Split(".")[1];
            var stream = System.IO.File.OpenRead($"./Assets/Images/{url}");
            
            return File(stream, $"image/{type}");
        }
    }
}