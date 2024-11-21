using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Common.Pages.Base
{
    internal class BackwardDummyPage() : CallbackQueryPageBase
    {
        public async override Task<PageResultBase> ViewAsync(Update update, UserState userState)
        {
            userState.Pages.Pop();
            return await userState.CurrentPage.ViewAsync(update, userState);
        } 
        public override ButtonLinkPage[][] GetKeyboard()
        {
            throw new NotImplementedException();
        }

        public override string GetText(UserState userState)
        {
            throw new NotImplementedException();
        }

    }
}
