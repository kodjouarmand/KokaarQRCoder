using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KokaarQrCoder.Domain.Entities
{
    public class Agency : BaseEntity<Guid>
    {
        [Required]
        public string Name { get; set; }

        [Required]        
        public string LocationUrl { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
    }
}
