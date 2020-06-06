using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace NewsBotTelegram.BackDatas
{
    class SourceBackData : BackData
    {
        public static InlineKeyboardMarkup keyboardNews;
        public override string Name => "infosource";

        public override async void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            Source source;
            if (e.CallbackQuery.Data.StartsWith("source"))
            source = user.Sources[int.Parse(e.CallbackQuery.Data.Substring(6))];
            else return;
            string url = GetUrl(source.Url.ToString());
            try
            {
                JArray jsonArray = JArray.Parse(await GetNews($"https://newsapiwork.azurewebsites.net/domain/{url}"));
                user.Articles = JsonConvert.DeserializeObject<Article[]>(jsonArray.ToString());
                if (user.Articles.Length == 0) throw new JsonReaderException();
                CreateKeyBoard(user.Articles);
                user.Command = "infoNews";
                await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Click to know more information:", replyMarkup: keyboardNews);
            }
            catch (JsonReaderException exeption)
            {
                System.Console.WriteLine(exeption.Message);
                await client.SendTextMessageAsync(e.CallbackQuery.From.Id, $"Error!!!\n" +
                     $"Sorry, but Api cannot find news");
                user.Command = "fromdata";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                try
                {
                    JArray jsonArray = JArray.Parse(await GetNews($"https://newsapiwork.azurewebsites.net/domain/{url.Substring(url.IndexOf('.')+1)}"));
                    user.Articles = JsonConvert.DeserializeObject<Article[]>(jsonArray.ToString());
                    CreateKeyBoard(user.Articles);
                    user.Command = "infoNews";
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Click to know more information:", replyMarkup: keyboardNews);
                }
                catch (Exception exep)
                {
                    Console.WriteLine(exep.Message);
                    await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Sorry, but there is some problem with this site:(\n" +
                        "Rechoose please!)");

                }
            }
        }
        private static string GetUrl(string oldurl)
        {
            string s;
            if (oldurl.Contains("www.")) 
            s = oldurl.Substring(oldurl.IndexOf("www.")+4);
            else s= oldurl.Substring(oldurl.IndexOf("https://") + 8);
            s = s.Substring(0, s.LastIndexOf('/'));
            return s;
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
