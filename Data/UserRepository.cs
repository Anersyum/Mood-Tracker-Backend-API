using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        public UserRepository(DataContext context)
        {
            this.context = context;

        }
        public void EditUser(User user)
        {
            throw new System.NotImplementedException();
        }

        public async void DeleteUser(User user)
        {
            this.context.Users.Remove(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            List<User> users = await this.context.Users.ToListAsync();

            return users;
        }

        public async Task<User> GetUserViaId(int userId)
        {
            return await this.context.Users.Include(x => x.Moods).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}