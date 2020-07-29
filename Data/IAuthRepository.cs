using System.Threading.Tasks;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password);
         Task<User> Login(string username, string password);
         Task<bool> UserExits(string username, string email);
         bool ValidateEmail(string email);
    }
}