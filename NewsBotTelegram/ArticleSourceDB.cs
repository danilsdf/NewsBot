using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsBotTelegram
{
    public class ArticleDB
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
        public Uri UrlToImage { get; set; }
    }
    public class SourceDB
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Uri Url { get; set; }
    }
}
