using Telegram.Bot;
using Telegram.Bot.Types;

namespace NewsBotTelegram.Commands
{
    class AboutThingCommand : Command
    {
        public override string Name => @"/infothing";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);

            user.Command = "THING";

            await client.SendTextMessageAsync(message.Chat, $"Please, write a name of thing, about which you want to know a news!)");
        }
    }
}
