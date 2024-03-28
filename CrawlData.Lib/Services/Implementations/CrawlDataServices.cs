using Craw_Data_From_URL.Model;
using CrawlData.Lib.Data.Contexts;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CrawlData.Lib.Services.Implementations
{
    public class CrawlDataServices : ICrawlDataServices
    {

        private readonly ILogger<ICrawlDataServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _dbContext;



        public CrawlDataServices(ILogger<ICrawlDataServices> logger, IConfiguration configuration, ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        public async Task GetDataAsync(List<string> urls)
        {
            foreach (string url in urls)
            {
                try
                {

                    var httpClient = _httpClientFactory.CreateClient();
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string html = await response.Content.ReadAsStringAsync();
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(html);

                        HtmlNode titleNode = doc.DocumentNode.SelectSingleNode("//span[@class='cms_blue']");
                        HtmlNode descriptionNode = doc.DocumentNode.SelectSingleNode("//h2[@class='intro']");
                        HtmlNode contentNode = doc.DocumentNode.SelectSingleNode("//div[@id='newscontent']");

                        if (titleNode != null && descriptionNode != null && contentNode != null)
                        {
                            var item = new DataItem
                            {
                                Title = titleNode.InnerText.Trim(),
                                Description = descriptionNode.InnerText.Trim(),
                                Content = contentNode.InnerText.Trim()
                            };
                            await SaveDataAsync(item);
                        }
                        else
                        {
                            _logger.LogError("Data not found for URL: {Url}", url);
                        }
                    }
                    else
                    {
                        _logger.LogError("HTTP request failed with status code: {StatusCode} for URL: {Url}", response.StatusCode, url);
                    }
                }


                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing URL: {Url}", url);
                }
            }
        }

        public async Task<List<string>> GetLinkFromURL(string urlGet)
        {
            List<string> urls = new List<string>();
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = await httpClient.GetAsync(urlGet);
                if (response.IsSuccessStatusCode)
                {
                    string html = await response.Content.ReadAsStringAsync();
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var links = doc.DocumentNode.SelectNodes("//div[@id='divTopEvents']//li/a");
                    if (links != null)
                    {
                        foreach (var link in links)
                        {
                            string href = link.GetAttributeValue("href", "");
                            urls.Add("https://s.cafef.vn" + href);
                        }
                    }
                    else
                    {
                        _logger.LogError("Data not found in HTML for URL: {Url}", urlGet);
                    }
                }
                else
                {
                    _logger.LogError("HTTP request failed with status code: {StatusCode} for URL: {Url}", response.StatusCode, urlGet);
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching URL: {Url}", urlGet);
            }
            return urls;
        }

        public async Task SaveDataAsync(DataItem item)
        {
            try
            {
                _dbContext.DataItems.Add(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving data");
            }
        }
    }
}
