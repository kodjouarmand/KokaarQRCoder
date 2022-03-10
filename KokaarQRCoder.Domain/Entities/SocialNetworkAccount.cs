using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KokaarQrCoder.Domain.Entities
{
    public class SocialNetworkAccount : BaseEntity<Guid>
    {
        [Required]
        public string Account { get; set; }

        [Required]
        public Guid SocialNetworkId { get; set; }
        [ForeignKey("SocialNetworkId")]
        public SocialNetwork SocialNetwork { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
    }
}
