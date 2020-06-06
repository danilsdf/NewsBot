using System;

namespace NewsBotTelegram
{
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
