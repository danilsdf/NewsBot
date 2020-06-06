using System.Collections.Generic;

namespace NewsBotTelegram
{
    public class UserSettings
    {
        public long Id { get; set; }
        public string ChatId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Category { get; set; }
        public string Command { get; set; }
        public bool edit = false;
        public Article[] Articles;
        public Source[] Sources;
        public string thing;
        public string FromDate;
        public string ToDate;
    }
}
