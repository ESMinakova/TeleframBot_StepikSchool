using IRON_PROGRAMMER_BOT_Common.Pages.Base;
using IRON_PROGRAMMER_BOT_Common.Pages.Helpers;
using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.Pages.MozgokachalkaBranch
{
    public class SuccessfulRegistrationPage(IServiceProvider services) : CallbackQueryAndMessagePageBase
    {
        public override UserState ProcessMessage(Message message, UserState userState)
        {
            return userState;
        }

        public override IPage GetNextPage(Update update)
        {
            if (update?.Message?.Text == "/reset")
                return services.GetRequiredService<StartPage>();
            return this;
        }

        public override string GetText(UserState userState)
        {
            return Resources.SuccessfulRegistrationPageText;
        }

        public override ButtonLinkPage[][] GetKeyboard()
        {
            return
                [
                    [
                        new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("В начало"), services.GetRequiredService<StartPage>())
                    ],
                ];
        }
    }
}
