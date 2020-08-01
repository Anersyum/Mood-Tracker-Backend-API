using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialSite.API.Dto;
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
        public async Task<bool> EditUser(EditUserDto user)
        {
            try
            {
                var userToEdit = await this.context.Users.Where(x => x.Id == user.Id).FirstOrDefaultAsync();

                if (userToEdit == null)
                {
                    return false;
                }

                this.UpdateUser(userToEdit, user);

                this.context.Users.Update(userToEdit);
                await this.context.SaveChangesAsync();    
            }
            catch (System.Exception)
            {
                
                return false;
            }

            return true;
        }

        private bool UpdateUser(User userToUpdate, EditUserDto userNewInfo)
        {
            userToUpdate.Username = userNewInfo.Username;
            userToUpdate.Email = userNewInfo.Email;
            userToUpdate.FirstName = userNewInfo.FirstName;
            userToUpdate.LastName = userNewInfo.LastName;
            userToUpdate.DateOfBirth = userNewInfo.DateOfBirth;
            userToUpdate.Bio = userNewInfo.Bio;
            userToUpdate.ProfileImagePath = userNewInfo.ProfileImagePath;
            
            return true;
        }

        public async void DeleteUser(User user)
        {
            this.context.Users.Remove(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByUsername(string username)
        {
            username = username.ToLower();
            List<User> users = await this.context.Users.Where(x => x.Username.IndexOf(username) != -1).ToListAsync();

            return users;
        }

        public async Task<User> GetUserViaId(int userId)
        {
            return await this.context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}