using IRON_PROGRAMMER_BOT_Common.User;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace IRON_PROGRAMMER_BOT_Common.Pages.PageResults
{
    public class PageResultBase(string text, IReplyMarkup? replyMarkup)
    {
        public string Text { get; } = text;
        public IReplyMarkup? ReplyMarkup { get; } = replyMarkup;
        public UserState UpdatedUserState { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Html;

        public bool IsMedia => this is PhotoPageResult ||
                                this is AudioPageResult ||
                                this is DocumentPageResult;



    }
}