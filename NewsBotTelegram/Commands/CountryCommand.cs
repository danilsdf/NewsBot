using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NewsBotTelegram.Commands
{
    class CountryCommand : Command
    {
        public static InlineKeyboardMarkup keyboardCountry = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                                      {
                        new [] { InlineKeyboardButton.WithCallbackData("Ukraine", "ua"),InlineKeyboardButton.WithCallbackData("Russia", "ru") },
                        new [] { InlineKeyboardButton.WithCallbackData("United Kingdom", "gb"),InlineKeyboardButton.WithCallbackData("Canada", "ca") },
                        new [] { InlineKeyboardButton.WithCallbackData("France", "fr"),InlineKeyboardButton.WithCallbackData("Germany", "de") },
                        new [] { InlineKeyboardButton.WithCallbackData("New Zealand", "nz"),InlineKeyboardButton.WithCallbackData("United States", "us") },
                        });
        public override string Name => @"/country";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            user.Command = "country";
            await client.SendTextMessageAsync(
                 message.Chat.Id, $"Please, choose your country\n" +
                 $"And be carefull, because news of country will be sended in national language of this country", replyMarkup: keyboardCountry);
        }
    }
}
