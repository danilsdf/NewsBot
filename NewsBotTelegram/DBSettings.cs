using System.Net.Http;
using System.Threading.Tasks;

namespace NewsBotTelegram
{
    public class DBSettings
    {
        public static UserContextForDB DB = new UserContextForDB();
        public static async Task<string> GetNews(string URL)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(URL);
            HttpContent content = response.Content;
            string result = await content.ReadAsStringAsync();
            return result;
        }
    }
}
