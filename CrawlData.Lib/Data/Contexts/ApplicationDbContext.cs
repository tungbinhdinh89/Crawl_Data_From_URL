using Craw_Data_From_URL.Model;
using Microsoft.EntityFrameworkCore;


namespace CrawlData.Lib.Data.Contexts
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<DataItem> DataItems { get; set; }
    }
}
