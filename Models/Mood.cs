using System;

namespace SocialSite.API.Models
{
    public class Mood
    {
        public int Id { get; set; }
        public int MoodValue { get; set; }
        public DateTime MoodRecordedDate { get; set; } 
        public User User { get; set; }
        public int UserId { get; set; }
    }
}