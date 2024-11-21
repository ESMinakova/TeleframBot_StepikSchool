using IRON_PROGRAMMER_BOT_Common.Pages.Base;
using IRON_PROGRAMMER_BOT_Common.StepikApi;
using IRON_PROGRAMMER_BOT_Common.User;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.Pages.CoursesBranch
{
    public class CSharpPromocodePage(IServiceProvider services) : CallbackQueryAndMessagePageBase
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
            var text =  Resources.CSharpPromocodePageText;
            var stepikAPIProvider = services.GetRequiredService<StepikApiProvider>();
            var promocodes = stepikAPIProvider.GetPromoCodesAsync(userState.UserData.SelectedCourseId!).Result;
            promocodes.ForEach(promocode => text += $@"{promocode.Name}
            ");
            return text;
        }

        public override ButtonLinkPage[][] GetKeyboard()
        {

            return [[new ButtonLinkPage(InlineKeyboardButton.WithCallbackData("Назад"), services.GetRequiredService<BackwardDummyPage>())]];
     
        }
    }
}