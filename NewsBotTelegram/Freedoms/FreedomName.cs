using Telegram.Bot;
using Telegram.Bot.Types;

namespace NewsBotTelegram
{
    class FreedomName : FreedomSMS
    {
        public override string Name => "THING";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            user.thing = message.Text;
            user.Command = "fromdata";
            await client.SendTextMessageAsync(message.Chat.Id, $"Please, write a begining date of news\n" +
                 $"(example: 2020-05-10)");
        }
    }
}
