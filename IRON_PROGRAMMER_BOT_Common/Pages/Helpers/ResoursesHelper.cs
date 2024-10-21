using Telegram.Bot.Types;

namespace IRON_PROGRAMMER_BOT_Common.Pages.Helpers
{
    public class ResoursesHelper
    {
        public  InputFileStream GetResourse(byte[] content, string fileName = "FileName")
        {
            var memoryStream = new MemoryStream(content);
            return InputFile.FromStream(memoryStream, fileName);
         
        }
    }
}
