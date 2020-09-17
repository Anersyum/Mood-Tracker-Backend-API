using System;
using Microsoft.AspNetCore.Http;

namespace SocialSite.API.Dto
{
    public class EditUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public IFormFile ProfileImage { get; set; }
        public string Email { get; set; }
    }
}