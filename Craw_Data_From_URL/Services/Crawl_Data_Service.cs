using Craw_Data_From_URL.Model;
using HtmlAgilityPack;
using Microsoft.Data.SqlClient;

namespace Craw_Data_From_URL.Services
{
    public class Crawl_Data_Service
    {
        private static string connectionString = "Server=TUNGBINHDINH89\\SQLEXPRESS;Database=CrawlData;Trusted_Connection=True;TrustServerCertificate=true;";
        public static async Task<DataItem?> GetDataAsync(string url)
        {
            try
            {
                var handler = new HttpClientHandler();
                using (var client = new HttpClient(handler))
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string html = await response.Content.ReadAsStringAsync();
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(html);

                        HtmlNodeCollection divNodes = doc.DocumentNode.SelectNodes("div");
                        string xpath = $"//*[contains(@class,'article-title')]";
                        if (doc.DocumentNode != null)
                        {
                            HtmlNode titleNode = doc.DocumentNode.SelectSingleNode($"//*[contains(@class,'article-title')]");
                            HtmlNode descriptionNode = doc.DocumentNode.SelectSingleNode($"//*[contains(@class,'txt-head')]");
                            HtmlNode contentNode = doc.DocumentNode.SelectSingleNode($"//*[contains(@class,'article-content')]");

                            if (titleNode != null && descriptionNode != null && contentNode != null)
                            {
                                return new DataItem { Title = titleNode.InnerText.Trim(), Description = descriptionNode.InnerText.Trim(), Content = contentNode.InnerText.Trim() };
                            }
                            else
                            {
                                Console.WriteLine("Data not found");
                                return null;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Document node not found");
                            return null;
                        }
                    }
                    else
                    {
                        throw new HttpRequestException($"HTTP request failed with status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public static async Task SaveDataAsync(DataItem item)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (var context = new ApplicationDbContext())
                {
                    context.DataItems.Add(item);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
