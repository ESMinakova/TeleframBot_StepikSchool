using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Common.Pages
{
    public interface IPage
    {
        Task<PageResultBase> ViewAsync(Update update, UserState userState);
        Task<PageResultBase> HandleAsync(Update update, UserState userState);
    }
}
