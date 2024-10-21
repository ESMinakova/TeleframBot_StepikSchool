using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace IRON_PROGRAMMER_BOT_ConsoleApp
{
    public class LongPollingConfigurator(ITelegramBotClient botClient, IUpdateHandler updateHandler) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var user = await botClient.GetMeAsync();

            Console.WriteLine($"Начали слушать апдейты с {user.Username}");

            await botClient.ReceiveAsync(updateHandler: updateHandler);
        }
    }
}
