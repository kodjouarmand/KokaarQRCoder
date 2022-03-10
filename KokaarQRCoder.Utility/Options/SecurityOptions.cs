using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KokaarQrCoder.Utility.Options
{
    public class SecurityOptions
    {
        public const string ConfigSectionName = "SecurityOptions:Identity";

        public int? RequiredLength { get; set; } = 6;
        public bool RequireDigit { get; set; } = true;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireNonAlphanumeric { get; set; } = true;
        public int? RequiredUniqueChars { get; set; } = 1;
        public bool RequireUniqueEmail { get; set; } = true;
        public int? MaxFailedAccessAttempts { get; set; } = 5;
        public int? DefaultLockoutTimeSpan { get; set; } = 5;
        public bool RequireConfirmedAccount { get; set; } = false;
        public bool RequireConfirmedEmail { get; set; } = false;
        public bool RequireConfirmedPhoneNumber { get; set; } = false;
    }
}
