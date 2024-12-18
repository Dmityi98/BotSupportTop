using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace TelegramBot
{
    public class Program
    {
        private static ITelegramBotClient _botClient;

        private static ReceiverOptions _receiverOptions;

        static async Task Main()
        {
            _botClient = new TelegramBotClient("8182465060:AAGHEWkAPvea4uMr0tFykj7rcUP0cfW_rPs"); // Токен телеграмм бота 

            _receiverOptions = new ReceiverOptions 
            {
                AllowedUpdates = new[] 
                {
                UpdateType.Message,
                },
            };
            using var cts = new CancellationTokenSource();
            _botClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions);

            var user = await _botClient.GetMe();
            Console.WriteLine($"{user.FirstName} запушен");

            await Task.Delay(-1);// Задежка для постоянной работы бота

        }

        private static async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                switch (update.Type)
                {
                    case UpdateType.Message:
                    {
                        var message = update.Message;
                        if (message.Text.ToLower() == "/start")
                        {
                            var chat = message.Chat;
                            var user = message.From;
                            await botClient.SendMessage(chat.Id, "Добро пожаловать" +
                                " в телеграмм бота помошника AcademyTop");
                        }
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}