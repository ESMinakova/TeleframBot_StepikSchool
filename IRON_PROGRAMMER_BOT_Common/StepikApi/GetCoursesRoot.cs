using Newtonsoft.Json;

namespace IRON_PROGRAMMER_BOT_Common.StepikApi
{
    public class GetCoursesRoot
    {
        [JsonProperty("courses")] 
        public List<Course> Courses { get; set; }
    }
}
