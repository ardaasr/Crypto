using Microsoft.EntityFrameworkCore;

namespace Crypto.Models
{
    public class DataContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-6N26MAE\\SQLEXPRESS;Database=CRYPTO; Integrated Security=True; TrustServerCertificate=True;");
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
    }
}
