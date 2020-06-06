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
    [Route("[controller]/{country}")]
    [ApiController]
    public class SourceController : ControllerBase
    {
        public async Task<Source[]> Get(string country)
        {
            JObject search = JObject.Parse(await GetNews($"https://newsapi.org/v2/sources?country={country}&apiKey=b442d901cc94427f8786ca3772744e13"));
            Sourses news = JsonConvert.DeserializeObject<Sourses>(search.ToString());
            return news.Sources;
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
    public class Sourses
    {
        public Source[] Sources { get; set; }
    }

    public class Source
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Uri Url { get; set; }

    }
}