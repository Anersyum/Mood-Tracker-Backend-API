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

        [HttpPost("save")]
        public async Task<IActionResult> SaveAction(MoodDto moodToSave)
        {
            int moodValue = Convert.ToInt32(moodToSave.MoodValue);
            int userId = Convert.ToInt32(moodToSave.UserId);

            if (moodValue > 2 || moodValue < 0)
            {
                return BadRequest("Mood does not exist");
            }

            Mood mood = new Mood
            {
                UserId = userId,
                MoodValue = moodValue
            };

            mood.MoodRecordedDate = DateTime.Now;
            await this.moodRepository.SaveMood(mood);

            return StatusCode(201, mood);
        }

        [HttpGet("get/{userId}")]
        public async Task<IActionResult> GetMonthlyUserMoodsAction(int userId)
        {
            var moods = await this.moodRepository.GetMonthlyUserMoodStatistics(userId);

            return Ok(moods);
        }
    }
}