using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SocialSite.API.Data;
using SocialSite.API.Dto;
using SocialSite.API.Models;

namespace SocialSite.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MoodController : ControllerBase
    {
        private readonly IMoodRepository moodRepository;
        private readonly IConfiguration config;

        public MoodController(IMoodRepository moodRepository, IConfiguration config)
        {
            this.config = config;
            this.moodRepository = moodRepository;
        }

        [HttpGet("get/{moodName}")]
        public async Task<IActionResult> GetMoodsAction(string moodName)
        {
            var moodsList = await this.moodRepository.FindMoods(moodName);

            return Ok(moodsList);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveAction(MoodDto moodToSave)
        {
            var userMood = await this.moodRepository.SaveMood(moodToSave);

            if (userMood == null)
            {
                return BadRequest("Mood does not exist");
            }
            

            return StatusCode(201, userMood);
        }

        // [HttpGet("get/{userId}")]
        // public async Task<IActionResult> GetMonthlyUserMoodsAction(int userId)
        // {
        //     var moods = await this.moodRepository.GetMonthlyUserMoodStatistics(userId);

        //     return Ok(moods);
        // }
    }
}