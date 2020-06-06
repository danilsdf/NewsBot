using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NewsBotTelegram.Commands
{
    public abstract class Command : DBSettings 
    {
        public abstract string Name { get; }
        public abstract void Execute(Message message, TelegramBotClient client, long id);
        public bool Contains(string command)
        {
            return command.Contains(this.Name);
        }
        
    }
}
