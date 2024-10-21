using IRON_PROGRAMMER_BOT_Common.Pages;

namespace IRON_PROGRAMMER_BOT_Common.User
{
    public sealed record class UserState(Stack<IPage> Pages, UserData UserData)
    {
        public IPage CurrentPage => Pages.Peek();

        public void AddPage(IPage page)
        {
            if (CurrentPage.GetType() != page.GetType())
                Pages.Push(page);
        }
    }

}