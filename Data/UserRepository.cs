using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialSite.API.Dto;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserRepository(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;

        }
        public async Task<User> EditUser(EditUserDto user)
        {
            try
            {
                var userToEdit = await this.context.Users.Where(x => x.Id == user.Id).FirstOrDefaultAsync();

                if (userToEdit == null)
                {
                    return null;
                }

                this.UpdateUser(userToEdit, user);

                if (user.ProfileImage.Length > 0)
                {
                    var contentType = user.ProfileImage.ContentType.Split("/")[1];
                    string[] allowedFileTypes = new string[] { "jpg", "jpeg", "png" };
                    // todo: secure the file upload add to settings host
                    if (allowedFileTypes.Contains(contentType))
                    {
                        string newFileName = Path.GetRandomFileName().Split(".")[0];
                        string filePath = $"./Assets/Images/{newFileName}.{contentType}";
                        string host = $"{this.httpContextAccessor.HttpContext.Request.Scheme}://{this.httpContextAccessor.HttpContext.Request.Host}";
                        
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await user.ProfileImage.CopyToAsync(stream);
                            userToEdit.ProfileImagePath = $"{host}/api/users/user/{user.Id}/{newFileName}.{contentType}";
                        }
                    }
                }

                this.context.Users.Update(userToEdit);
                await this.context.SaveChangesAsync();

                return userToEdit;
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }

        private bool UpdateUser(User userToUpdate, EditUserDto userNewInfo)
        {
            userToUpdate.Username = userNewInfo.Username;
            userToUpdate.Email = userNewInfo.Email;
            userToUpdate.FirstName = userNewInfo.FirstName;
            userToUpdate.LastName = userNewInfo.LastName;
            userToUpdate.DateOfBirth = userNewInfo.DateOfBirth;
            userToUpdate.Bio = userNewInfo.Bio;

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