using IRON_PROGRAMMER_BOT_Common.Pages.Base;
using IRON_PROGRAMMER_BOT_Common.Pages.CoursesBranch;
using IRON_PROGRAMMER_BOT_Common.Pages.Helpers;
using IRON_PROGRAMMER_BOT_Common.Pages.MozgokachalkaBranch;
using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.Pages
{
    public class StartPage(ResoursesHelper resoursesHelper, IServiceProvider services) : CallbackQueryPhotoPageBase(resoursesHelper)
    {

        public override byte[] GetPhoto()
        {
            return Resources.bot_3;
        }

        public override string GetText()
        {
            return Resources.StartPageText;
        }

        public override ButtonLinkPage[][] GetKeyboard()
        {
            return
                [
                    [
                        new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("МОЗГОКАЧАЛКА"), services.GetRequiredService<MozgokacahlkaPage>())
                    ],
                    [
                        new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("Наши курсы"), services.GetRequiredService<CommonCoursesPage>())
                    ]
                ];
        }    


    }
}
