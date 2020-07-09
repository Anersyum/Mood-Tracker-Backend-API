using System.Collections.Generic;
using System.Threading.Tasks;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public interface IDiaryRepository
    {
         Task<ICollection<Diary>> GetAllUserDiaryEntries(int userId);
         Task<Diary> SaveNewDiaryEntry(Diary diary);
         Task<Diary> GetDiaryEntry(int diaryId, int userId); 
    }
}