using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NewsApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        [HttpGet("{domain}")]
        public async Task<Article[]> Get(string domain)
        {
            JObject search = JObject.Parse(await GetNews($"http://newsapi.org/v2/everything?domains={domain}&apiKey=b442d901cc94427f8786ca3772744e13"));
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
}