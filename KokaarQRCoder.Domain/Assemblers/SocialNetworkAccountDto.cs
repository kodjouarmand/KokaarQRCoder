using System;
using System.ComponentModel.DataAnnotations;
using KokaarQrCoder.Domain.Entities;

namespace KokaarQrCoder.Domain.Assemblers
{
    public class SocialNetworkAccountDto : BaseDto<Guid>
    {
        [Required]
        public string Account { get; set; }

        [Required]
        [Display(Name = "Social Network")]
        public Guid SocialNetworkId { get; set; }
        public SocialNetworkDto SocialNetwork { get; set; }

        public Guid CompanyId { get; set; }
        public CompanyDto Company { get; set; }
    }
}
