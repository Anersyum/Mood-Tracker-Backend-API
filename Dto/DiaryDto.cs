namespace SocialSite.API.Dto
{
    public class DiaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Entry { get; set; }
        public int UserId { get; set; }
    }
}