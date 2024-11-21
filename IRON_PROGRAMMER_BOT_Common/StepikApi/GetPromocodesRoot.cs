using System.Text.Json.Serialization;

namespace IRON_PROGRAMMER_BOT_Common.StepikApi
{
    public class GetPromocodesRoot
    {
        [JsonPropertyName("promo-codes")]
        public List<Promocode>? Promocodes { get; set; }
    }
    
}
