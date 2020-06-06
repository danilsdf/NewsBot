using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace NewsBotTelegram.BackDatas
{
    class NewsInfoBackData : BackData
    {
        public static InlineKeyboardMarkup keyboardUrl; 
        public override string Name => "infoNews";

        public override async void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            Article article = null;
            try
            {
                if (e.CallbackQuery.Data.StartsWith("News"))
                    article = user.Articles[int.Parse(e.CallbackQuery.Data.Substring(4))];
                else return;
                CreateButton(article);
                //user.Command = " ";
                if (article.UrlToImage == null)
                {if(article.Description == null)
                        await client.SendTextMessageAsync(e.CallbackQuery.From.Id, article.Title, replyMarkup: keyboardUrl);
                    else
                        await client.SendTextMessageAsync(e.CallbackQuery.From.Id, article.Description, replyMarkup: keyboardUrl);
                }
                else
                {
                    InputOnlineFile photo = new InputOnlineFile(article.UrlToImage);
                    await client.SendPhotoAsync(e.CallbackQuery.From.Id, photo, caption: article.Description, replyMarkup: keyboardUrl);
                }
            }
            catch (InvalidOperationException exeption)
            {
                Console.WriteLine(exeption.Message);
                await client.SendTextMessageAsync(e.CallbackQuery.From.Id, article.Description, replyMarkup: keyboardUrl);

            }
            catch (ApiRequestException exeption)
            {
                Console.WriteLine(exeption.Message);
                await client.SendTextMessageAsync(e.CallbackQuery.From.Id, article.Description, replyMarkup: keyboardUrl);

            }
            catch (Exception exeption)
            {
                Console.WriteLine(exeption.Message);
                await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "There is some problem\n" +
                    "Use any command to fix it)");
            }
        }
        private void CreateButton(Article article)
        {
            keyboardUrl = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                                 {
                                         new [] { InlineKeyboardButton.WithUrl("To see on original site", article.Url.ToString()),},
               });
        }
    }
}
