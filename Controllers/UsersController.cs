using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        // [HttpGet("get/{id}")]
        // public async Task<IActionResult> GetUserByIdAction(int id)
        // {
        //     var userToReturn = await this.userRepo.GetUserViaId(id);

        //     DetailedUserDto user = this.mapper.Map<DetailedUserDto>(userToReturn);
            
        //     return Ok(user);
        // }

        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteUser(UserDto user)
        {
            return Ok("User deleted.");
        }
    }
}