using Microsoft.EntityFrameworkCore;
using SocialSite.API.Models;

namespace SocialSite.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Mood> Moods { get; set; }
        public DbSet<Diary> Diary { get; set; }
    }
}