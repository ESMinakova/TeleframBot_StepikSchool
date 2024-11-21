using IRON_PROGRAMMER_BOT_Common.Pages.Base;
using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.StepikApi;
using IRON_PROGRAMMER_BOT_Common.User;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.Pages.CoursesBranch
{
    public class CSharpCoursesPage(IServiceProvider services) : CallbackQueryAndMessagePageBase
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
            return Resources.CSharpCoursesPageText;
        }

        public override ButtonLinkPage[][] GetKeyboard()
        {
            var stepikAPIProvider = services.GetRequiredService<StepikApiProvider>();
            var courses = stepikAPIProvider.GetCoursesAsync(596721262).Result;

            return courses.Select<Course, ButtonLinkPage[]>(x =>
            [
                new ButtonLinkPage(InlineKeyboardButton.WithCallbackData(x.Title, x.Id.ToString()), services.GetRequiredService<CSharpPromocodePage>())
            ])
            .Concat([[new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("Назад"), services.GetRequiredService<BackwardDummyPage>())]])
            .ToArray();
        }

        public async override Task<PageResultBase> HandleAsync(Update update, UserState userState)
        {
            if (update.CallbackQuery == null)
            {
                return await ViewAsync(update, userState);
            }

            userState.UserData.SelectedCourseId = update.CallbackQuery?.Data;   
            return await base.HandleAsync(update, userState);
        }
    }
}