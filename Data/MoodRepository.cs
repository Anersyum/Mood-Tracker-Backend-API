using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SocialSite.API.Models;
using System.Collections.Generic;
using System.Linq;

namespace SocialSite.API.Data
{
    public class MoodRepository : IMoodRepository
    {

        private readonly DataContext context;
        public MoodRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Mood>> GetAllUserMoods(int userId)
        {
            var moodsFromUser =  await this.context.Moods.Where(x => x.UserId == userId).ToListAsync();

            return moodsFromUser;
        }

        public async Task<Mood> SaveMood(Mood mood)
        {
            await this.context.Moods.AddAsync(mood);
            await this.context.SaveChangesAsync();

            return mood;
        }
    }
}