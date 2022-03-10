using System;
using System.ComponentModel.DataAnnotations;

namespace KokaarQrCoder.Domain.Assemblers
{
    public class SocialNetworkDto : BaseDto<Guid>
    {
        [Required]
        public string Name { get; set; }
    }
}
