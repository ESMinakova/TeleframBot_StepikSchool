using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Common.Pages.Base
{
    public abstract class CallbackQueryAndMessagePageBase: CallbackQueryPageBase
    {
        public abstract UserState ProcessMessage(Message message, UserState userState);

        public abstract IPage GetNextPage(Update update);

        public async override Task<PageResultBase> HandleAsync(Update update, UserState userState)
        {
            if (update == null)
            {
                return await ViewAsync(update, userState);
            }
            if (update.Message == null)
            {
                var buttons = GetKeyboard().SelectMany(x => x);
                var pressedButton = buttons.FirstOrDefault(x => x.button.CallbackData == update.CallbackQuery.Data)!;

                return await pressedButton.page.ViewAsync(update, userState);
            }

            var updatedUserState = ProcessMessage(update.Message, userState);
            var nextPage = GetNextPage(update);

            return await nextPage.ViewAsync(update, updatedUserState);
        }
    }

}
