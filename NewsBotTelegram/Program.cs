using NewsBotTelegram.BackDatas;
using NewsBotTelegram.Commands;
using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace NewsBotTelegram
{
    class Program : DBSettings
    {
        private static TelegramBotClient BotClient;
        public static IReadOnlyList<Command> commands;
        public static IReadOnlyList<BackData> callbacks;
        public static IReadOnlyList<FreedomSMS> freedoms;
        private static bool here = false, did;
        //public static UserContext DB = new UserContext();
        static void Main()
        {

            BotClient = BotSettings.Get();
            commands = BotSettings.Commands;
            callbacks = BotSettings.CallBacks;
            freedoms = BotSettings.FreedomSMs;

            BotClient.OnMessage += BotClient_OnMessage;
            BotClient.OnMessageEdited += BotClient_OnMessage;
            BotClient.OnCallbackQuery += BotClient_OnCallbackQuery;

            BotClient.StartReceiving();
            Console.ReadKey();
        }
        private static async void BotClient_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == null || e.Message.Type != MessageType.Text) return;
            //return;
            //if (e.Message.Chat.Id != 386219611) return;
            var users = DB.Users;
            var message = e.Message;
            try
            {
                foreach (UserSettings u in users) { if (u.ChatId == message.Chat.Id.ToString()) here = true; }

                if (!here)
                {
                    DB.Users.Add(new UserSettings { ChatId = message.Chat.Id.ToString(), Name = message.Chat.FirstName });

                    await DB.SaveChangesAsync();
                    users = DB.Users;
                }
                did = false;
                foreach (UserSettings u in users)
                {
                    if (u.ChatId == message.Chat.Id.ToString())
                    {
                        foreach (var command in commands)
                        {
                            if (command.Contains(message.Text))
                            {
                                did = true;
                                command.Execute(message, BotClient, u.Id);
                                break;
                            }
                        }
                        foreach (var freedom in freedoms)
                        {
                            if (!did & freedom.Contains(u.Command))
                            {
                                did = true;
                                freedom.Execute(message, BotClient, u.Id);
                                break;
                            }
                        }
                        if (!did)
                        {
                            await BotClient.SendTextMessageAsync(message.Chat.Id, "I don`t understand you!");
                        }
                    }
                }
                Console.WriteLine($"{DateTime.Now} the {message.Chat.FirstName}({message.Chat.Id}) Write: {message.Text}");
                await DB.SaveChangesAsync();
            }
            catch (Exception exeption)
            {
                Console.WriteLine(exeption.Message);
                await BotClient.SendTextMessageAsync(message.Chat.Id, "There is some problem\n" +
                    "Use any command to fix it)");
            }

        }
        private static async void BotClient_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var users = DB.Users;
            try
            {
                foreach (UserSettings u in users)
                {
                    if (u.ChatId == e.CallbackQuery.From.Id.ToString())
                    {
                        foreach (var callback in callbacks)
                        {
                            if (callback.Contains(u.Command))
                            {
                                callback.Execute(e, BotClient, u.Id);
                                break;
                            }
                        }
                    }
                }
                Console.WriteLine($"{DateTime.Now} the {e.CallbackQuery.From.FirstName}({e.CallbackQuery.From.Id}) Select: {e.CallbackQuery.Data}");
                await DB.SaveChangesAsync();
            }
            catch (Exception exeption)
            {
                Console.WriteLine(exeption.Message);
                await BotClient.SendTextMessageAsync(e.CallbackQuery.From.Id, "There is some problem\n" +
                    "Use any command to fix it)");
            }
        }

    }
}
