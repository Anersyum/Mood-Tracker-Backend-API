using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SocialSite.API.Models;
using System.Collections.Generic;
using System.Linq;
using System;

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
    }
}