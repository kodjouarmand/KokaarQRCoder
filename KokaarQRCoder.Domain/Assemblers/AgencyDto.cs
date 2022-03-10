using System;
using System.ComponentModel.DataAnnotations;
using KokaarQrCoder.Domain.Entities;

namespace KokaarQrCoder.Domain.Assemblers
{
    public class AgencyDto : BaseDto<Guid>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Location URL")]
        public string LocationUrl { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        public CompanyDto Company { get; set; }
    }
}
