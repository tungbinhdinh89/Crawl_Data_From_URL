using CrawlData.Lib.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Craw_Data_From_URL
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var urlString = "https://s.cafef.vn/hose/htn-cong-ty-co-phan-hung-thinh-incons.chn";

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });


            IConfigurationRoot configuration = new ConfigurationBuilder()
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .Build();

            var crawlService = new CrawlDataServices(loggerFactory, configuration);

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
