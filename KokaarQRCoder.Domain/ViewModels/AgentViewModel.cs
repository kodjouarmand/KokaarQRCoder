using System.Collections.Generic;
using KokaarQrCoder.Domain.Assemblers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KokaarQrCoder.Domain.ViewModels
{
    public class AgentViewModel
    {
        public AgentDto Agent { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
        public bool HasQRCode { get; set; }
    }
}
