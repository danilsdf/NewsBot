using Telegram.Bot;
using Telegram.Bot.Types;

namespace NewsBotTelegram.Commands
{
    class StartCommand : Command
    {
        public override string Name => @"/start";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            user.Command = "LANGUAGE";
            user.Category = null;
            user.Country = null;
            await client.SendTextMessageAsync(message.Chat, $"Hi, {message.Chat.FirstName}, I can help you to know a news from a lot of countries\n" +
                $"Moreover, you can select a category of news\n" +
                $"/country - to set a country\n" +
                $"/category - to set a category\n" +
                $"/source - to select a source\n" +
                $"/infothing - to know all news from date to date\n" +
                $"/news - to know news");
        }
    }
}
