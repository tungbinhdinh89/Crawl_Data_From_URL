using Craw_Data_From_URL.Model;
using Microsoft.EntityFrameworkCore;

namespace Craw_Data_From_URL.Services
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<DataItem> DataItems { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=TUNGBINHDINH89\\SQLEXPRESS;Database=CrawlData;Trusted_Connection=True;TrustServerCertificate=true;");
        }
    }
}
