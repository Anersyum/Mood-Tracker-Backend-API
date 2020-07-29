using System;
using System.Threading.Tasks;
using SocialSite.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SocialSite.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;

        public AuthRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            User user = await this.context.Users.FirstOrDefaultAsync(x => x.Username == username || x.Email == username);

            if (user == null || !VerifyUserPassword(password, user.PasswordHash, user.PasswordSat)) 
            {
                return null;
            }

            return user;
        }

        private bool VerifyUserPassword(string password, byte[] passwordHash, byte[] passwordSat)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSat)) 
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int  i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) 
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash;
            byte[] passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSat = passwordSalt;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) 
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExits(string username, string email)
        {
            return await this.context.Users.AnyAsync(x => x.Username == username || x.Email == email);
        }

        public bool ValidateEmail(string email)
        {
            if (email.IndexOf("@") == -1)
            {
                return false;
            }

            return true;
        }
    }
}