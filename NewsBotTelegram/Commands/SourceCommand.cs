using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NewsBotTelegram.Commands
{
    class SourceCommand : Command
    {
        public static InlineKeyboardMarkup keyboardSources;
        public static InlineKeyboardMarkup keyboardCountry = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                     {
                        new [] { InlineKeyboardButton.WithCallbackData("Ukraine", "ua"),InlineKeyboardButton.WithCallbackData("Russia", "ru") },
                        new [] { InlineKeyboardButton.WithCallbackData("United Kingdom", "gb"),InlineKeyboardButton.WithCallbackData("Canada", "ca") },
                        new [] { InlineKeyboardButton.WithCallbackData("France", "fr"),InlineKeyboardButton.WithCallbackData("Germany", "de") },
                        new [] { InlineKeyboardButton.WithCallbackData("New Zealand", "nz"),InlineKeyboardButton.WithCallbackData("United States", "us") },
                       });
        public override string Name => @"/source";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            user.Command = "SOURCE";
            if(user.Country == null)
            {
                user.Command = "country";
                await client.SendTextMessageAsync(
                     message.Chat.Id, $"Please, choose your country\n" +
                     $"And be carefull, because news of country will be sended in national language of this country", replyMarkup: keyboardCountry);
                return;
            }
            JArray jsonArray = JArray.Parse(await GetNews($"https://newsapiwork.azurewebsites.net/source/{user.Country}"));
            Source[] sourses = JsonConvert.DeserializeObject<Source[]>(jsonArray.ToString());
            if (sourses.Length < 2)
            {
                await client.SendTextMessageAsync(message.Chat.Id, "Sorry, but i haven`t got a link of sources for you country:(");
                return;
            }
            user.Sources = sourses;
            CreateKeyBoard(user.Sources);
            user.Command = "infosource";
            await client.SendTextMessageAsync(message.Chat.Id, "Click to select a source of news:", replyMarkup: keyboardSources);
        }
        private void CreateKeyBoard(Source[] sources)
        {
            if(sources.Length >= 2 && sources.Length < 4)
            {
                keyboardSources = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                          {
                        new [] { InlineKeyboardButton.WithCallbackData(sources[0].Name,"source0"),},
                        new [] { InlineKeyboardButton.WithCallbackData(sources[1].Name, "source1"),},
                                          });
            }
            else if (sources.Length >= 4 && sources.Length <6)
            { 
                keyboardSources = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                          {
                        new [] { InlineKeyboardButton.WithCallbackData(sources[0].Name,"source0"),},
                        new [] { InlineKeyboardButton.WithCallbackData(sources[1].Name, "source1"),},
                        new [] { InlineKeyboardButton.WithCallbackData(sources[2].Name, "source2"),},
                        new [] { InlineKeyboardButton.WithCallbackData(sources[3].Name, "source3"),},
                                          });
            }
            else if(sources.Length >= 6)
            {
                keyboardSources = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                          {
                        new [] { InlineKeyboardButton.WithCallbackData(sources[0].Name, "source0"),},
                        new [] { InlineKeyboardButton.WithCallbackData(sources[1].Name, "source1"),},
                        new [] { InlineKeyboardButton.WithCallbackData(sources[2].Name, "source2"),},
                        new [] { InlineKeyboardButton.WithCallbackData(sources[3].Name, "source3"),},
                        new [] { InlineKeyboardButton.WithCallbackData(sources[4].Name, "source4"),},
                        new [] { InlineKeyboardButton.WithCallbackData(sources[5].Name, "source5"),},
                                          });
            }
        }
    }
}
