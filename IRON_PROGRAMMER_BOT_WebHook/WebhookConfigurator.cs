
using IRON_PROGRAMMER_BOT_Common.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace IRON_PROGRAMMER_BOT_WebHook
{
    public class WebhookConfigurator(ITelegramBotClient botClient, IOptions<BotConfiguration> botConfiguration) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var webhookAddress = botConfiguration.Value.HostAddress + BotConfiguration.UpdateRoute;
            await botClient.SetWebhookAsync(
                url: webhookAddress,
                secretToken: botConfiguration.Value.SecretToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await botClient.DeleteWebhookAsync();
        }
    }
}
