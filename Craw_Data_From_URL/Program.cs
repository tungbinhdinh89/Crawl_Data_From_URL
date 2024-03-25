using Craw_Data_From_URL.Services;
using System.Text;

namespace Craw_Data_From_URL
{
    internal class Program
    {
        Crawl_Data_Service crawl = new Crawl_Data_Service();
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string url = "https://antt.nguoiduatin.vn/co-phieu-hpx-chinh-thuc-duoc-giao-dich-tro-lai-tren-san-chung-khoan-tu-ngay-203-bien-do-dao-dong-len-toi-20-11255.html";
            Crawl_Data_Service.GetDataAsync(url);
            Console.ReadLine();
        }
    }
}
