using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.Pages.Base
{
    public abstract class CallbackQueryPageBase: IPage
    {
        public abstract string GetText(UserState userState);

        public abstract ButtonLinkPage[][] GetKeyboard();


        public async virtual Task<PageResultBase> ViewAsync(Update update, UserState userState)
        {
            var text = GetText(userState);


            var replyMarkup = GetInlineKeybpardMarkup();

            userState.AddPage(this);
            return new PageResultBase(text, replyMarkup)
            {
                UpdatedUserState = userState
            };
        }

        public async virtual Task<PageResultBase> HandleAsync(Update update, UserState userState)
        {
            if (update.CallbackQuery == null)
            {
                return await ViewAsync(update, userState);
            }
            var buttons = GetKeyboard().SelectMany(x => x);
            var pressedButton = buttons.FirstOrDefault(x => x.button.CallbackData == update.CallbackQuery.Data)!;

            return await pressedButton.page.ViewAsync(update, userState);
        }

        protected InlineKeyboardMarkup GetInlineKeybpardMarkup()
        {
            return new InlineKeyboardMarkup(GetKeyboard().Select(page => page.Select(x => x.button)));
        }
    }

}
