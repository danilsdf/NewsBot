using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NewsBotTelegram.Commands
{
    class NewsCommand : Command
    {
        public static InlineKeyboardMarkup keyboardCountry = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                      {
                        new [] { InlineKeyboardButton.WithCallbackData("Ukraine", "ua"),InlineKeyboardButton.WithCallbackData("Russia", "ru") },
                        new [] { InlineKeyboardButton.WithCallbackData("United Kingdom", "gb"),InlineKeyboardButton.WithCallbackData("Canada", "ca") },
                        new [] { InlineKeyboardButton.WithCallbackData("France", "fr"),InlineKeyboardButton.WithCallbackData("Germany", "de") },
                        new [] { InlineKeyboardButton.WithCallbackData("New Zealand", "nz"),InlineKeyboardButton.WithCallbackData("United States", "us") },
                        });
        public static InlineKeyboardMarkup keyboardCategory = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                      {
                        new [] { InlineKeyboardButton.WithCallbackData("Business", "business"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Entertainment", "entertainment"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Health", "health"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Science", "science"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Sport", "sports"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Technology", "technology"),},
    });
        public static InlineKeyboardMarkup keyboardNews;
        public override string Name => @"/news";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            try
            {
                if (user.Country != null && user.Category != null)
                {
                    JArray jsonArray = JArray.Parse(await GetNews($"https://newsapiwork.azurewebsites.net/news/{user.Country}/{user.Category}"));
                    user.Articles = JsonConvert.DeserializeObject<Article[]>(jsonArray.ToString());
                    if (user.Articles.Length == 0) throw new JsonReaderException();
                    CreateKeyBoard(user.Articles);
                    user.Command = "infoNews";
                    await client.SendTextMessageAsync(message.Chat.Id, "Click to know more information:", replyMarkup: keyboardNews);
                }
                else
                {
                    if (user.Country == null)
                    {
                        user.Command = "country";
                        await client.SendTextMessageAsync(
                             message.Chat.Id, $"Please, choose your country\n" +
                             $"And be carefull, because news of country will be sended in national language of this country", replyMarkup: keyboardCountry);
                        return;
                    }
                    if (user.Category == null)
                    {
                        user.Command = "category";
                        await client.SendTextMessageAsync(
                             message.Chat.Id, $"Please, choose a category of news", replyMarkup: keyboardCategory);
                        return;
                    }
                }
            }
            catch (JsonReaderException exeption)
            {
                System.Console.WriteLine(exeption.Message);
                await client.SendTextMessageAsync(message.Chat.Id, $"Error!!!\n" +
                     $"Sorry, but Api cannot find news");
                user.Command = "fromdata";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                user.Country = null;
                user.Category = null;
                await client.SendTextMessageAsync(message.Chat, $"Please select:\n" +
                $"/country - to set a country\n" +
                $"/category - to set a category");
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
            }else if (articles.Length >= 4 && articles.Length < 7)
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
