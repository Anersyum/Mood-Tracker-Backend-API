using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public class DiaryRepository : IDiaryRepository
    {
        private readonly DataContext context;

        public DiaryRepository(DataContext context)
        {
            this.context = context;
            
        }
        public async Task<ICollection<Diary>> GetAllUserDiaryEntries(int userId)
        {
            return await this.context.Diary.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Diary> GetDiaryEntry(int diaryId, int userId)
        {
            return await this.context.Diary.FirstOrDefaultAsync(x => (x.Id == diaryId && x.UserId == userId));
        }

        public async Task<Diary> SaveNewDiaryEntry(Diary diary)
        {
            diary.DateRecorded = DateTime.Now;
            await this.context.Diary.AddAsync(diary);
            await this.context.SaveChangesAsync();

            return diary;
        }
    }
}