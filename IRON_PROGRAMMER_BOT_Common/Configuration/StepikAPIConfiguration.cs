using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRON_PROGRAMMER_BOT_Common.Configuration
{
    public class StepikAPIConfiguration
    {
        public const string SectionName = "StepikAPIConfiguration";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
