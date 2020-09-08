using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialSite.API.Dto;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public class DiaryRepository : IDiaryRepository
    {
        private readonly DataContext context;
        private readonly int numberOfEntriesPerPage = 10;
        public DiaryRepository(DataContext context)
        {
            this.context = context;
            
        }
        public async Task<ICollection<Diary>> GetAllUserDiaryEntries(int userId, int page)
        {
            return await this.context.Diary.Where(x => x.UserId == userId)
                            .OrderBy(x => x.DateRecorded)
                            .Skip((page - 1) * this.numberOfEntriesPerPage)
                            .Take(page * this.numberOfEntriesPerPage)
                            .ToListAsync();
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

        public async Task<bool> DeleteDiaryEntry(DiaryDto diaryEntryDto)
        {
            var diaryEntry = await this.GetDiaryEntry(diaryEntryDto.Id, diaryEntryDto.UserId);

            try 
            {
                this.context.Diary.Remove(diaryEntry);
                await this.context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> EditDiaryEntry(DiaryDto diaryEntry)
        {
            try 
            {
                Diary foundDiaryEntry = await this.context.Diary
                                            .FirstOrDefaultAsync(x => x.Id == diaryEntry.Id);

                if (foundDiaryEntry == null)
                {
                    return false;
                }

                foundDiaryEntry.Title = diaryEntry.Title;
                foundDiaryEntry.Entry = diaryEntry.Entry;

                this.context.Diary.Update(foundDiaryEntry);
                await this.context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public async Task<ICollection<Diary>> GetFilteredUserDiaryEntreis(int month, int year, int page)
        {
            DateTime endDateOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);
            DateTime beginingOfMonth = new DateTime(year, month, 1);

            try
            {
                var entries = await this.context.Diary.Where(x => 
                    x.DateRecorded >= beginingOfMonth && x.DateRecorded <= endDateOfMonth)
                        .OrderBy(x => x.DateRecorded)
                        .Skip((page - 1) * this.numberOfEntriesPerPage)
                        .Take(page * this.numberOfEntriesPerPage).ToListAsync();
                
                return entries;
            }
            catch(Exception e) 
            {
                return null;
            }
        }
    }
}