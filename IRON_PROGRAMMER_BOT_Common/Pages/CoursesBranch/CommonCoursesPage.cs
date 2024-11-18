using IRON_PROGRAMMER_BOT_Common.Pages.Base;
using IRON_PROGRAMMER_BOT_Common.User;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.Pages.CoursesBranch
{
    public class CommonCoursesPage(IServiceProvider services) : CallbackQueryAndMessagePageBase
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
            return Resources.CommonCoursesPageText;
        }

        public override ButtonLinkPage[][] GetKeyboard()
        {
            return
                [
                    [
                        new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("Подготовка к ЕГЭ и ОГЭ"), services.GetRequiredService<OgeAndEgePage>())
                    ],
                                        [
                        new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("Курсы"), services.GetRequiredService<CSharpCoursesPage>())
                    ],
                    //[
                    //    new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("Войти в Айти"), services.GetRequiredService<NotStatedPage>())
                    //],
                    //[
                    //    new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("Профессия - разработчик"), services.GetRequiredService<NotStatedPage>())
                    //]
                ];
        }
    }
}