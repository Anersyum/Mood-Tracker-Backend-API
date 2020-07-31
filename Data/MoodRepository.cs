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

        public async Task<IEnumerable<int>> GetMonthlyUserMoodStatistics(int userId)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            
            DateTime endDateOfMonth = new DateTime(year, month, daysInMonth, 23, 59, 59);
            DateTime beginingOfMonth = new DateTime(year, month, 1);

            var deppressedMoods =  await this.context.Moods.Where(x =>
                x.UserId == userId && 
                    (x.MoodRecordedDate >= beginingOfMonth && x.MoodRecordedDate <= endDateOfMonth) && x.MoodValue == 0
            ).CountAsync();
            
            var contentMoods =  await this.context.Moods.Where(x =>
                x.UserId == userId && 
                    (x.MoodRecordedDate >= beginingOfMonth && x.MoodRecordedDate <= endDateOfMonth) && x.MoodValue == 1
            ).CountAsync();

            var happyMoods =  await this.context.Moods.Where(x =>
                x.UserId == userId && 
                    (x.MoodRecordedDate >= beginingOfMonth && x.MoodRecordedDate <= endDateOfMonth) && x.MoodValue == 2
            ).CountAsync();

            List<int> moodStatistics = new List<int>() {
                deppressedMoods, contentMoods, happyMoods
            };

            return moodStatistics;
        }

        public async Task<Mood> SaveMood(Mood mood)
        {
            await this.context.Moods.AddAsync(mood);
            await this.context.SaveChangesAsync();

            return mood;
        }
    }
}