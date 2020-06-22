using System.Collections.Generic;
using System.Threading.Tasks;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public interface IMoodRepository
    {
         Task<Mood> SaveMood(Mood mood);
         Task<IEnumerable<Mood>> GetAllUserMoods(int userId);  
    }
}