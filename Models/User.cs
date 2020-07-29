using System;
using System.Collections.Generic;

namespace SocialSite.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        public string ProfileImagePath { get; set; }
        public ICollection<Mood> Moods { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSat { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}