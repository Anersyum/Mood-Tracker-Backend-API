using System.Collections.Generic;
using System.Threading.Tasks;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public interface IUserRepository
    {
         Task<IEnumerable<User>> GetUsersByUsername(string username);
         Task<User> GetUserViaId(int userId);
         void EditUser(User user);

         void DeleteUser(User user);
    }
}