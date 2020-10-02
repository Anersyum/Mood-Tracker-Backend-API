using System;
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

                if (user.ProfileImage != null && (user.ProfileImage.Length / Math.Pow(1024, 2)) < 2.0)
                {
                    var contentType = user.ProfileImage.ContentType.Split("/")[1];
                    string[] allowedFileTypes = new string[] { "jpg", "jpeg", "png" };

                    if (allowedFileTypes.Contains(contentType))
                    {
                        string newFileName = Path.GetRandomFileName().Split(".")[0];
                        string filePath = $"./Assets/Images/{newFileName}.{contentType}";
                        
                        using (var stream = System.IO.File.Create(filePath))
                        {
                            await user.ProfileImage.CopyToAsync(stream);
                            userToEdit.ProfileImagePath = $"{newFileName}.{contentType}";
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

            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                var user = await this.context.Users.FirstOrDefaultAsync(x => x.Id == id);

                this.context.Users.Remove(user);
                await this.context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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