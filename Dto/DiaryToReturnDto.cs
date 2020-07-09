using System;

namespace SocialSite.API.Dto
{
    public class DiaryToReturnDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Entry { get; set; }
        public int UserId { get; set; }
        public DateTime DateRecorded { get; set; }
    }
}