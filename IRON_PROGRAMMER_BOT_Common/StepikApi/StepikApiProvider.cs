using System.Net.Http.Json;

namespace IRON_PROGRAMMER_BOT_Common.StepikApi
{
    public partial class StepikApiProvider
    {
        private readonly HttpClient httpClient;
        public StepikApiProvider(HttpClient httpClient)
        {
            this.httpClient = httpClient;       
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
    }
}
