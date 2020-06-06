using Telegram.Bot;
using Telegram.Bot.Types;

namespace NewsBotTelegram.Freedoms
{
    class FreedomFDate : FreedomSMS
    {
        public override string Name => "fromdata";

        public override async void Execute(Message message, TelegramBotClient client, long id)
        {
            var user = DB.Users.Find(id);
            if (message.Text.IndexOf('-') == 4 && message.Text.LastIndexOf('-') == 7)
            {
                user.FromDate = message.Text;
                user.Command = "todata";
                await client.SendTextMessageAsync(message.Chat.Id, $"Please, write a finish date of news\n" +
                 $"(example: 2020-05-10)");
            }
            else
            {
                await client.SendTextMessageAsync(message.Chat.Id, $"Error, write like example\n" +
                 $"(example: 2020-05-10)");
            }
        }
    }
}