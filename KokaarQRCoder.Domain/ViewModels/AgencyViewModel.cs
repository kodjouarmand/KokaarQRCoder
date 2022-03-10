using System.Collections.Generic;
using KokaarQrCoder.Domain.Assemblers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KokaarQrCoder.Domain.ViewModels
{
    public class AgencyViewModel
    {
        public AgencyDto Agency { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}
