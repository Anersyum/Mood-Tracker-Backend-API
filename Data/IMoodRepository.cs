using System.Collections.Generic;
using System.Threading.Tasks;
using SocialSite.API.Dto;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public interface IMoodRepository
    {
         Task<UserMoods> SaveMood(MoodDto mood);
        //  Task<IEnumerable<int>> GetMonthlyUserMoodStatistics(int userId);  
        Task<IEnumerable<Mood>> FindMoods(string moodName);
    }
}