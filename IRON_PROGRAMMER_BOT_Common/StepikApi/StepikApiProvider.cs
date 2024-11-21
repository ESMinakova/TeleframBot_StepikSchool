using IRON_PROGRAMMER_BOT_Common.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace IRON_PROGRAMMER_BOT_Common.StepikApi
{
    public partial class StepikApiProvider
    {
        private readonly HttpClient httpClient;
        private readonly StepikAPIConfiguration configurationOptions;
        private string accessToken;
        private DateTime tokenExpiration;


        public StepikApiProvider(HttpClient httpClient, IOptions<StepikAPIConfiguration> configurationOptions)
        {
            this.httpClient = httpClient;
            this.configurationOptions = configurationOptions.Value;
        }

        public async Task<List<Course>> GetCoursesAsync(int teacherId)
        {
            try
            {
                var root = await httpClient.GetFromJsonAsync<GetCoursesRoot>
                    ($"https://stepik.org/api/courses?is_censored=false&is_unsuitable=false&order=-popularity&page=1&teacher={teacherId}");
                return root?.Courses ?? [];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return [];
            }
        }
        public async Task<List<Promocode>> GetPromoCodesAsync(string courseID)
        {
            try
            {
                await EnsureAuthenticatedAsync();
                var root = await httpClient.GetFromJsonAsync<GetPromocodesRoot>
                    ($"https://stepik.org/api/promo-codes?course={courseID}&is_stepik_promo=false&order=-id&page=1");
                return root?.Promocodes ?? [];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return [];
            }
        }

        public async Task AuthenticateAsync()
        {
            var authUrl = "https://stepik.org/oauth2/token/";
            var authData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string> ("grant_type", "client_credentials"),
                new KeyValuePair<string, string> ("client_id", configurationOptions.ClientId),
                new KeyValuePair<string, string> ("client_secret", configurationOptions.ClientSecret),
            ]);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, authUrl)
            {
                Content = authData
            };

            requestMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var response = await httpClient.SendAsync(requestMessage);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponse>(jsonResponse);

            accessToken = authResponse.AccessToken;
            tokenExpiration = DateTime.UtcNow.AddSeconds(authResponse.ExpiresIn);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                 
        }

        private async Task EnsureAuthenticatedAsync()
        {
            if (DateTime.Now <= tokenExpiration)
            {
                await AuthenticateAsync();
            }
        }
    }
}
