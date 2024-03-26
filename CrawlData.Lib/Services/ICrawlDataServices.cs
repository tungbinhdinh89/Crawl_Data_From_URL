using Craw_Data_From_URL.Model;

namespace CrawlData.Lib.Services
{
    internal interface ICrawlDataServices
    {

        Task GetDataAsync(List<string> urls);

        Task<List<string>> GetLinkFromURL(string urlGet);

        Task SaveDataAsync(DataItem item);
    }
}
