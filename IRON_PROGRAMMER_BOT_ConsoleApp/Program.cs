using IRON_PROGRAMMER_BOT_Common;
using IRON_PROGRAMMER_BOT_ConsoleApp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

class Program
{

    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args).ConfigureServices((context, services) =>
        {
            ContainerConfigurator.Configure(context.Configuration, services);
            services.AddHostedService<LongPollingConfigurator>();

        }).Build();

        await host.RunAsync();
    } 
}
