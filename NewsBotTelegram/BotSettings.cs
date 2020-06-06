using NewsBotTelegram.BackDatas;
using NewsBotTelegram.Commands;
using NewsBotTelegram.Freedoms;
using System.Collections.Generic;
using Telegram.Bot;

namespace NewsBotTelegram
{
    class BotSettings
    {
        public static TelegramBotClient client;
        public static string token = "1177826824:AAGMa5b6mGmn0RZFOvZyKwNeZ8pctL8dOm0";
        public static string Name = "NewsbotWork_bot";

        private static List<Command> commandslist;
        public static IReadOnlyList<Command> Commands { get => commandslist.AsReadOnly(); }

        private static List<BackData> callbacklist;
        public static IReadOnlyList<BackData> CallBacks { get => callbacklist.AsReadOnly(); }

        private static List<FreedomSMS> freedomSMs;
        public static IReadOnlyList<FreedomSMS> FreedomSMs { get => freedomSMs.AsReadOnly(); }

        public static TelegramBotClient Get()
        {
            commandslist = new List<Command>();
            commandslist.Add(new StartCommand());
            commandslist.Add(new CountryCommand());
            commandslist.Add(new CategoryCommand());
            commandslist.Add(new SourceCommand());
            commandslist.Add(new NewsCommand());
            commandslist.Add(new AboutThingCommand());

            callbacklist = new List<BackData>();
            callbacklist.Add(new CategoryBackData());
            callbacklist.Add(new CountryBackData());
            callbacklist.Add(new SourceBackData());
            callbacklist.Add(new NewsInfoBackData());

            freedomSMs = new List<FreedomSMS>();
            freedomSMs.Add(new FreedomFDate());
            freedomSMs.Add(new FreedomTDate());
            freedomSMs.Add(new FreedomName());

            client = new TelegramBotClient(token);
            return client;
        }
    }
}
