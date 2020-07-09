using System;

namespace SocialSite.API.Models
{
    public class Diary
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Entry { get; set; }
        public DateTime DateRecorded { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}