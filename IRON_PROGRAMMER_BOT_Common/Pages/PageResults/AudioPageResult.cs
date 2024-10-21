using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


namespace IRON_PROGRAMMER_BOT_Common.Pages.PageResults
{
    public class AudioPageResult : PageResultBase
    {
        public InputFile Audio { get; set; }
        public AudioPageResult(InputFile audio, string text, IReplyMarkup? replyMarkup) : base(text, replyMarkup)
        {
            Audio = audio;
        }
    }
}
