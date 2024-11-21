using System.Text.Json.Serialization;

namespace IRON_PROGRAMMER_BOT_Common.StepikApi
{
    public class Promocode
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
    
}
