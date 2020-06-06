using System;

namespace NewsBotTelegram
{
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
