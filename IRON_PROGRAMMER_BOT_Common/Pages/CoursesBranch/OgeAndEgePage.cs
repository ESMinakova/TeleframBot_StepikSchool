using IRON_PROGRAMMER_BOT_Common.Pages.Base;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.Pages.CoursesBranch
{
    public class OgeAndEgePage(IServiceProvider services) : CallbackQueryPageBase
    {

        public override string GetText()
        {
            return Resources.OgeAndEgePageText;
        }

        public override ButtonLinkPage[][] GetKeyboard()
        {
            return
                [
                    [
                        new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("В начало"),  services.GetRequiredService<StartPage>())
                    ]
                ];
        }
    }
}
