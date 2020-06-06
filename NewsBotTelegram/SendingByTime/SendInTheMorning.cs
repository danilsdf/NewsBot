using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace NewsBotTelegram.SendingByTime
{
    class SendInTheMorning : DBSettings, IJob
    {
        public static InlineKeyboardMarkup keyboardNews;
        public static TelegramBotClient client = new TelegramBotClient(BotSettings.token);
        public async Task Execute(IJobExecutionContext context)
        {
            foreach (UserSettings user in DB.Users)
            {
                JArray jsonArray = JArray.Parse(await GetNews($"https://newsapiwork.azurewebsites.net/news/{user.Country}/{user.Category}"));
                user.Articles = JsonConvert.DeserializeObject<Article[]>(jsonArray.ToString());
                CreateKeyBoard(user.Articles);
                user.Command = "infoNews";
                await client.SendTextMessageAsync(user.ChatId, "Good morning:)\n" +
                    "It`s new news for today!" +
                    "\nClick to know more information:", replyMarkup: keyboardNews);
            }
        }
        private void CreateKeyBoard(Article[] articles)
        {
            if (articles.Length >= 2 && articles.Length < 4)
            {
                keyboardNews = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                          {
                        new [] { InlineKeyboardButton.WithCallbackData(articles[0].Title,"News0"),},
                        new [] { InlineKeyboardButton.WithCallbackData(articles[1].Title, "News1"),},
                                          });
            }
            else if (articles.Length >= 4 && articles.Length < 7)
            {
                keyboardNews = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                          {
                        new [] { InlineKeyboardButton.WithCallbackData(articles[0].Title,"News0"),},
                        new [] { InlineKeyboardButton.WithCallbackData(articles[1].Title, "News1"),},
                        new [] { InlineKeyboardButton.WithCallbackData(articles[2].Title, "News2"),},
                        new [] { InlineKeyboardButton.WithCallbackData(articles[3].Title, "News3"),},
                                          });
            }
            else if (articles.Length >= 7)
            {
                keyboardNews = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                          {
                        new [] { InlineKeyboardButton.WithCallbackData(articles[0].Title,"News0"),},
                        new [] { InlineKeyboardButton.WithCallbackData(articles[1].Title, "News1"),},
                        new [] { InlineKeyboardButton.WithCallbackData(articles[2].Title, "News2"),},
                        new [] { InlineKeyboardButton.WithCallbackData(articles[3].Title, "News3"),},
                        new [] { InlineKeyboardButton.WithCallbackData(articles[4].Title, "News4"),},
                        new [] { InlineKeyboardButton.WithCallbackData(articles[5].Title, "News5"),},
                        new [] { InlineKeyboardButton.WithCallbackData(articles[5].Title, "News6"),}
                                          });
            }
        }
    }
}
