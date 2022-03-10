using System.Collections.Generic;
using KokaarQrCoder.Domain.Assemblers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KokaarQrCoder.Domain.ViewModels
{
    public class CompanyViewModel
    {
        public CompanyDto Company { get; set; }
        public IEnumerable<AgencyDto> Agencies { get; set; }
        public IEnumerable<SocialNetworkAccountDto> SocialNetworkAccounts { get; set; }
        public bool HasQRCode { get; set; }
    }
}
