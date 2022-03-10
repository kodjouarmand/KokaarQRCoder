using System;
using System.ComponentModel.DataAnnotations;
using KokaarQrCoder.Domain.Entities;

namespace KokaarQrCoder.Domain.Assemblers
{
    public class AgentDto : BaseDto<Guid>
    {
        [Required]
        public string Number { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        public CompanyDto Company { get; set; }

    }
}
