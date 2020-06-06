using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewsApi.Controllers
{
    [Route("[controller]/{country}")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        [HttpGet("{category}")]
        public async Task<Article[]> Get(string category, string country)
        {
            JObject search = JObject.Parse(await GetNews($"http://newsapi.org/v2/top-headlines?country={country}&category={category}&apiKey=b442d901cc94427f8786ca3772744e13"));
            News news = JsonConvert.DeserializeObject<News>(search.ToString());
            return news.Articles;
        }
        public static async Task<string> GetNews(string URL)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(URL);
            HttpContent content = response.Content;
            string result = await content.ReadAsStringAsync();
            return result;
        }
    }
    public class News
    {
        public Article[] Articles { get; set; }
    }

    public class Article
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
        public Uri UrlToImage { get; set; }
    }
}