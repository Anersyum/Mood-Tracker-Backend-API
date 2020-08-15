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

        public async Task<IEnumerable<Mood>> FindMoods(string moodName)
        {
            if (moodName == "empty")
            {
                return await this.context.Moods.ToListAsync();
            }
            
            return await this.context.Moods.Where(x => x.MoodName.IndexOf(moodName) != -1).ToListAsync();
        }

        public async Task<UserMoods> SaveMood(MoodDto moodToSave)
        {
            int userId =  Convert.ToInt32(moodToSave.UserId);
            int moodId = Convert.ToInt32(moodToSave.MoodId);

            var mood = await this.context.Moods.FirstOrDefaultAsync(x => x.Id == moodId);

            if (mood == null)
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
    }
}