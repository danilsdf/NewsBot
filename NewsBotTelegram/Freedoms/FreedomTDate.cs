using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NewsBotTelegram.Freedoms
{
    class FreedomTDate : FreedomSMS
    {
        public static InlineKeyboardMarkup keyboardNews;
        public override string Name => "todata";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            try
            {
                if (message.Text.IndexOf('-') == 4 && message.Text.LastIndexOf('-') == 7)
                {
                    user.ToDate = message.Text;
                    JArray jsonArray = JArray.Parse(await GetNews($"https://newsapiwork.azurewebsites.net/mention/{user.thing}/{user.FromDate}/{user.ToDate}"));
                    user.Articles = JsonConvert.DeserializeObject<Article[]>(jsonArray.ToString());
                    if (user.Articles.Length == 0) throw new JsonReaderException();
                    CreateKeyBoard(user.Articles);
                    user.Command = "infoNews";
                    await client.SendTextMessageAsync(message.Chat.Id, "Click to know more information:", replyMarkup: keyboardNews);
                }
                else
                {
                    await client.SendTextMessageAsync(message.Chat.Id, $"Error, write like example\n" +
                     $"(example: 2020-05-10)");
                }
            }
            catch(JsonReaderException exeption)
            {
                System.Console.WriteLine(exeption.Message);
                await client.SendTextMessageAsync(message.Chat.Id, $"Error!!!\n" +
                     $"Sorry, but Api cannot find news between these dates");
                user.Command = "fromdata";
                await client.SendTextMessageAsync(message.Chat.Id, $"Please, write a begining date of news\n" +
                     $"(example: 2020-05-10)");
            }
            catch (IndexOutOfRangeException exeption)
            {
                System.Console.WriteLine(exeption.Message);
                await client.SendTextMessageAsync(message.Chat.Id, $"Error!!!\n" +
                     $"Sorry, but Api cannot find news between these dates");
                user.Command = "fromdata";
                await client.SendTextMessageAsync(message.Chat.Id, $"Please, write a begining date of news\n" +
                     $"(example: 2020-05-10)");
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
                        new [] { InlineKeyboardButton.WithCallbackData(articles[6].Title, "News6"),}
                                          });
            }
        }
    }
}