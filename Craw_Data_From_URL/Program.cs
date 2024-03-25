using Microsoft.Extensions.Logging;
using System.Text;
using static Craw_Data_From_URL.Services.Crawl_Data_Service;

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

            var crawlService = new CrawlDataService(loggerFactory);

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
