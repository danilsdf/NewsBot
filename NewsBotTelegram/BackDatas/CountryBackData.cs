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
    class CountryBackData : BackData
    {
        public static InlineKeyboardMarkup keyboardCategory = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                         {
                        new [] { InlineKeyboardButton.WithCallbackData("Business", "business"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Entertainment", "entertainment"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Health", "health"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Science", "science"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Sport", "sports"),},
                        new [] { InlineKeyboardButton.WithCallbackData("Technology", "technology"),},
       });
        public override string Name => "country";

        public override async void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            user.Country = e.CallbackQuery.Data;
            if (user.Category == null)
            {
                user.Command = "category";
                await client.SendTextMessageAsync(
                 e.CallbackQuery.From.Id, $"Please, choose a category of news", replyMarkup: keyboardCategory);
            }
            else
            {
                user.Command = " ";
                await client.SendTextMessageAsync(e.CallbackQuery.From.Id, "Good, the country has been set!\n" +
                    "write /news for getting news");
            }
        }
    }
}
