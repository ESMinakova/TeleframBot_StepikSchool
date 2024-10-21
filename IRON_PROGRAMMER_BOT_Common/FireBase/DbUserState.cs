using IRON_PROGRAMMER_BOT_Common.User;

namespace IRON_PROGRAMMER_BOT_Common.FireBase
{
    public class DbUserState
    {
        public UserData UserData { get; set; }
        public List<string> PageNames { get; set; }
    }
}
