using System.Collections.Generic;
using System.Threading.Tasks;
using SocialSite.API.Dto;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public interface IUserRepository
    {
         Task<IEnumerable<User>> GetUsersByUsername(string username);
         Task<User> GetUserViaId(int userId);
         Task<User> EditUser(EditUserDto user);

         Task<bool> DeleteUser(int id);
    }
}