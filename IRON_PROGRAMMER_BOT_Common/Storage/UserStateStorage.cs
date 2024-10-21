using IRON_PROGRAMMER_BOT_Common.FireBase;
using IRON_PROGRAMMER_BOT_Common.Pages;
using IRON_PROGRAMMER_BOT_Common.User;

namespace IRON_PROGRAMMER_BOT_Common.Storage
{
    public sealed class UserStateStorage(FirebaseProvider firebaseProvider, PagesFactory pagesFactory)
    {
        public async Task AddOrUpdate(long telegramUserID, UserState userState)
        {
            var userStateFirebase = ToUserStateFirebase(userState);
            await firebaseProvider.AddOrUpdateAsync($"userStates/{telegramUserID}", userStateFirebase);
        }

        public async Task<UserState?> TryGetAsync(long telegramUserID)
        {
            var userStateFirebase = await firebaseProvider.TryGetAsync<DbUserState?>($"userStates/{telegramUserID}");
            if (userStateFirebase == null)
                return null;
            return ToUserState(userStateFirebase);
        }

        private UserState ToUserState(DbUserState userStateFirebase)
        {
            var pages = userStateFirebase.PageNames.Select(x => pagesFactory.GetPage(x)).Reverse();
            return new UserState(new Stack<IPage>(pages), userStateFirebase.UserData);
        }

        private static DbUserState ToUserStateFirebase(UserState userState)
        {
            return new DbUserState
            {
                UserData = userState.UserData,
                PageNames = userState.Pages.Select(x => x.GetType().FullName).ToList()
            };
        }
    }
}
