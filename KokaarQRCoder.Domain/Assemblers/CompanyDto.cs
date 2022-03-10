using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KokaarQrCoder.Domain.Assemblers
{
    public class CompanyDto : BaseDto<Guid>
    {
        [Required]        
        public string Name { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
       
        [Display(Name = "Web Site")]
        public string WebSite { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Additionnal Informations")]
        public string AdditionnalInformations { get; set; }
    }
}
