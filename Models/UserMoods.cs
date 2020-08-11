using System;

namespace SocialSite.API.Models
{
    public class UserMoods
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Mood Mood { get; set; }
        public int MoodId { get; set; }
        public DateTime DateRecorded { get; set; }
    }
}