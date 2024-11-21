using IRON_PROGRAMMER_BOT_Common.Pages.Helpers;
using IRON_PROGRAMMER_BOT_Common.Pages.PageResults;
using IRON_PROGRAMMER_BOT_Common.User;
using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Common.Pages.Base
{
    public abstract class CallbackQueryPhotoPageBase(ResoursesHelper resoursesHelper): CallbackQueryPageBase
    {
        public abstract byte[] GetPhoto();

        public async override Task<PageResultBase> ViewAsync(Update update, UserState userState)
        {
            var text = GetText(userState);
            var keyboard = GetInlineKeybpardMarkup();
            var photo = resoursesHelper.GetResourse(GetPhoto());

            userState.AddPage(this);
            return new PhotoPageResult(photo, text, keyboard)
            {
                UpdatedUserState = userState
            };
        }


    }

}
