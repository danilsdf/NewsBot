using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace NewsBotTelegram.BackDatas
{
    public abstract class BackData : DBSettings
    {
        public abstract string Name { get; }
        public abstract void Execute(CallbackQueryEventArgs e, TelegramBotClient client, long id);
        public bool Contains(string callback)
        {
            return callback.Contains(this.Name);
        }
    }
}
