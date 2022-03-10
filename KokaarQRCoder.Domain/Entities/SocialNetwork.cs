using System;
using System.ComponentModel.DataAnnotations;

namespace KokaarQrCoder.Domain.Entities
{
    public class SocialNetwork : BaseEntity<Guid>
    {
        [Display(Name = "Social Network Name")]
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
