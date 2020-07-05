using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SocialSite.API.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SocialSite.API.Data
{
    public class MoodRepository : IMoodRepository
    {

        private readonly DataContext context;
        public MoodRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Mood>> GetMothlyUserMoods(int userId)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            DateTime endDateOfMonth = new DateTime(year, month, daysInMonth);
            DateTime beginingOfMonth = new DateTime(year, month, 1);

            var moodsFromUser =  await this.context.Moods.Where(x =>
                x.UserId == userId && 
                    (x.MoodRecordedDate >= beginingOfMonth && x.MoodRecordedDate <= endDateOfMonth)
            ).ToListAsync();

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