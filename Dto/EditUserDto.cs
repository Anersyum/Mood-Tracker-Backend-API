using System;
using Microsoft.AspNetCore.Http;

namespace SocialSite.API.Dto
{
    public class EditUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}