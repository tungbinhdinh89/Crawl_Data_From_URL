using CrawlData.Lib.Data.Contexts;
using CrawlData.Lib.Services;
using CrawlData.Lib.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Craw_Data_From_URL
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var urlString = "https://s.cafef.vn/hose/htn-cong-ty-co-phan-hung-thinh-incons.chn";

            // runtime - stage

            var configuration = new ConfigurationBuilder()
                 //.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json")
                 .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddScoped<ICrawlDataServices, CrawlDataServices>();
            services.AddLogging(); // Register ILogger service
            services.AddHttpClient(); // Register IHttpClientFactory service

            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            IServiceProvider sp = services.BuildServiceProvider();

            var serviceProvider = services.BuildServiceProvider();

            var crawlService = sp.GetRequiredService<ICrawlDataServices>();

            try
            {
                var urls = await crawlService.GetLinkFromURL(urlString);
                await crawlService.GetDataAsync(urls);

                Console.WriteLine("Progress Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            Console.ReadLine();
        }
    }
}
