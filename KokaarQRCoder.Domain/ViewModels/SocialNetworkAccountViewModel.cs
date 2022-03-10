using System.Collections.Generic;
using KokaarQrCoder.Domain.Assemblers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KokaarQrCoder.Domain.ViewModels
{
    public class SocialNetworkAccountViewModel
    {
        public SocialNetworkAccountDto SocialNetworkAccount { get; set; }
        public IEnumerable<SelectListItem> SocialNetworkList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}
