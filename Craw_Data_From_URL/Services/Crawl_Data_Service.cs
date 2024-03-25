using HtmlAgilityPack;

namespace Craw_Data_From_URL.Services
{
    public class Crawl_Data_Service
    {
        public static async void GetDataAsync(string url)
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
                                string title = titleNode.InnerText.Trim();
                                Console.WriteLine($"Title: {titleNode.InnerText.Trim()}, Description : {descriptionNode.InnerText.Trim()}, Content: {contentNode.InnerText.Trim()}");
                            }
                            else
                            {
                                Console.WriteLine("Data not found");
                            }
                        }
                        else
                        {

                            Console.WriteLine("Document node not found");
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
                Console.WriteLine($"Lỗi: {ex.Message}");
                //return string.Empty;
            }
        }
    }
}
