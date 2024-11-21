using Firebase.Database;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using IRON_PROGRAMMER_BOT_Common.Configuration;
using IRON_PROGRAMMER_BOT_Common.FireBase;
using IRON_PROGRAMMER_BOT_Common.Storage;
using IRON_PROGRAMMER_BOT_Common.Pages.Helpers;
using IRON_PROGRAMMER_BOT_Common.Pages;
using System.Reflection;
using IRON_PROGRAMMER_BOT_Common.StepikApi;

namespace IRON_PROGRAMMER_BOT_Common
{
    public static class ContainerConfigurator
    {
        public static void Configure(IConfiguration configuration, IServiceCollection services)
        {
            var firebaseConfigurationSection = configuration.GetSection(FirebaseConfiguration.SectionName);
            services.Configure<FirebaseConfiguration>(firebaseConfigurationSection);

            var botConfigurationSection = configuration.GetSection(BotConfiguration.SectionName);
            services.Configure<BotConfiguration>(botConfigurationSection);

            var stepikAPIConfigurationSection = configuration.GetSection(StepikAPIConfiguration.SectionName);
            services.Configure<BotConfiguration>(stepikAPIConfigurationSection);

            services.AddSingleton<UserStateStorage>();
            services.AddSingleton<FirebaseProvider>();
            services.AddSingleton(services =>
            {
                var firebaseConfig = services.GetService<IOptions<FirebaseConfiguration>>()!.Value;
                return new FirebaseClient(firebaseConfig.BasePath, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(firebaseConfig.Secret)
                });
            });

            services.AddHttpClient("tg_bot_client").AddTypedClient<ITelegramBotClient>((httpClient, services) =>
            {
                var botConfig = services.GetService<IOptions<BotConfiguration>>()!.Value;
                var options = new TelegramBotClientOptions(botConfig.BotToken);
                return new TelegramBotClient(options, httpClient);
            });

            services.AddSingleton<IUpdateHandler, UpdateHandler>();
            services.AddSingleton<ResoursesHelper>();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes().Where(x => typeof(IPage).IsAssignableFrom(x) && !x.IsAbstract);
            foreach (var type in types)
            {
                services.AddSingleton(type);
            }
            services.AddSingleton<PagesFactory>();
            services.AddSingleton<StepikApiProvider>();
        }
    }
}
