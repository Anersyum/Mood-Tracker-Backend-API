using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SocialSite.API.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using SocialSite.API.Dto;

namespace SocialSite.API.Data
{
    public class MoodRepository: IMoodRepository
    {

        private readonly DataContext context;
        public MoodRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Mood>> GetAllMoods()
        {
            return await this.context.Moods.ToListAsync();
        }

        public async Task<IEnumerable<object>> GetMonthlyUserMoodStatistics(int userId)
        {
            if (!await this.UserExists(userId))
            {
                return null;
            }

            var userMoods = await this.CountMoods();

            return userMoods;
        }

        private async Task<bool> UserExists(int userId)
        {
            var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return false;
            }

            return true;
        }

        private async Task<IEnumerable<object>> CountMoods()
        {
            List<object> userMoods = new List<object>();
            var allMoods = await this.context.Moods.ToListAsync();
            int i = 1;

            foreach (var mood in allMoods)
            {
                int moodCount = await this.context.UserMoods.Where(x => x.MoodId == i).CountAsync();
                i++;

                if (moodCount == 0)
                {
                    continue;
                }

                userMoods.Add(new 
                {
                    count = moodCount,
                    moodName = mood.MoodName
                });
            }

            return userMoods;
        }

        public async Task<UserMoods> SaveMood(MoodDto moodToSave)
        {
            int userId =  Convert.ToInt32(moodToSave.UserId);
            int moodId = Convert.ToInt32(moodToSave.MoodId);

            if (!await this.MoodExists(userId, moodId))
            {
                return null;
            }

            UserMoods userMood = new UserMoods() 
            {
                UserId = userId,
                MoodId = moodId,
                DateRecorded = DateTime.Now
            };

            await this.context.UserMoods.AddAsync(userMood);
            await this.context.SaveChangesAsync();

            return userMood;
        }

        private async Task<bool> MoodExists(int userId, int moodId)
        {
            var mood = await this.context.Moods.FirstOrDefaultAsync(x => x.Id == moodId);

            if (mood == null)
            {
                return false;
            }

            return true;
        }
    }
}