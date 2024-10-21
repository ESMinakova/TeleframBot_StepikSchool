using Firebase.Database;
using Firebase.Database.Query;

namespace IRON_PROGRAMMER_BOT_Common.FireBase
{
    public class FirebaseProvider(FirebaseClient client)
    {
        public async Task<T> TryGetAsync<T>(string key)
        {
            return await client.Child(key).OnceSingleAsync<T>();
        }
        public async Task AddOrUpdateAsync<T>(string key, T item)
        {
            await client.Child(key).PutAsync(item);
        }
    }
}
