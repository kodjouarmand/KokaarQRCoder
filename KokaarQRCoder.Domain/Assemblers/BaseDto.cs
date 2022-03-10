using System;

namespace KokaarQrCoder.Domain.Assemblers
{
    public abstract class BaseDto<TEntityKey>
    {
        public TEntityKey Id { get; set; }
        public DateTime? CreationDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime? LastModificationDate { get; set; }
        public string LastModificationUser { get; set; }
        //public byte[] RowVersion { get; set; }

        public bool IsNew() => Id.Equals(default(TEntityKey));
    }
}
