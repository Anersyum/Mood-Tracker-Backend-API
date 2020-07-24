using System.Collections.Generic;
using System.Threading.Tasks;
using SocialSite.API.Dto;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public interface IDiaryRepository
    {
         Task<ICollection<Diary>> GetAllUserDiaryEntries(int userId, int page);
         Task<Diary> SaveNewDiaryEntry(Diary diary);
         Task<Diary> GetDiaryEntry(int diaryId, int userId); 
         Task<bool> DeleteDiaryEntry(DiaryDto diaryEntry);
         Task<bool> EditDiaryEntry(DiaryDto diaryEntry);
    }
}