using IRON_PROGRAMMER_BOT_Common.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IRON_PROGRAMMER_BOT_Common.FireBase
{
    public class PagesFactory(IServiceProvider services)
    {
        public IPage GetPage(string typeName)
        {
            var type = Type.GetType(typeName) ?? throw new Exception("Такого типа нет в проекте");
            return (IPage)services.GetRequiredService(type);
 
        }
    }
}
