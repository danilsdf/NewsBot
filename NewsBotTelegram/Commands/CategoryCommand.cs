using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NewsBotTelegram.Commands
{
    class CategoryCommand : Command
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
        public override string Name => @"/category";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            user.Command = "category";
            await client.SendTextMessageAsync(
                 message.Chat.Id, $"Please, choose a category of news", replyMarkup: keyboardCategory);
        }
    }
}
