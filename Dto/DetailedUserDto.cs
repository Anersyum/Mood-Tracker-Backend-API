using System;

namespace SocialSite.API.Dto
{
    public class DetailedUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string ProfileImagePath { get; set; }
        public string Email { get; set; }
    }
}