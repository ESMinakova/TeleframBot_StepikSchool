using IRON_PROGRAMMER_BOT_Common.Pages.Base;
using IRON_PROGRAMMER_BOT_Common.User;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.Pages.MozgokachalkaBranch
{
    public class MozgokacahlkaPage(IServiceProvider services) : CallbackQueryAndMessagePageBase
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
            return Resources.MozgokachalkaPageText;
        }

        public override ButtonLinkPage[][] GetKeyboard()
        {
            return
                 [
                    [
                        new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("Правила участия"), services.GetRequiredService<MozgokachalkaRulesPage>())
                    ],
                    [
                        new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("Регистрация"), services.GetRequiredService<RegistrationPage>())
                    ],
                ];
        }
    }
}