using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Common.Pages
{
    public class NotStatedPage(IServiceProvider services) : IPage
    {
        public async Task<PageResultBase> ViewAsync(Update update, UserState userState)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResultBase> HandleAsync(Update update, UserState userState)
        {
            return await services.GetRequiredService<StartPage>().ViewAsync(update, userState);
        }
    }
}
