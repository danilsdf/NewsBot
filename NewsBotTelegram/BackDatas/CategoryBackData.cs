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
    class CategoryBackData : BackData
    {
        public static InlineKeyboardMarkup keyboardCountry = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                      {
                        new [] { InlineKeyboardButton.WithCallbackData("Ukraine", "ua"),InlineKeyboardButton.WithCallbackData("Russia", "ru") },
                        new [] { InlineKeyboardButton.WithCallbackData("United Kingdom", "gb"),InlineKeyboardButton.WithCallbackData("Canada", "ca") },
                        new [] { InlineKeyboardButton.WithCallbackData("France", "fr"),InlineKeyboardButton.WithCallbackData("Germany", "de") },
                        new [] { InlineKeyboardButton.WithCallbackData("New Zealand", "nz"),InlineKeyboardButton.WithCallbackData("United States", "us") },
                        });
        public override string Name => "category";

        public override async void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            user.Category = e.CallbackQuery.Data;
            if (user.Country == null)
            {
                user.Command = "country";
                await client.SendTextMessageAsync(
                     e.CallbackQuery.From.Id, $"Please, choose your country\n" +
                     $"And be carefull, because news of country will be sended in national language of this country", replyMarkup: keyboardCountry);
            }
            else
            {
                user.Command = " ";
                await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Good, the category has been set!\n" +
                    "write /news for getting news");
            }
        }
    }
}
