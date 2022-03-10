using KokaarQrCoder.Domain.Assemblers;
using System.Collections.Generic;
using KokaarQrCoder.BusinessLogic.Enums;

namespace KokaarQrCoder.BusinessLogic.Queries.Contracts
{
    public interface IBaseQuery<TDto, TEntityKey> where TDto : BaseDto<TEntityKey>
    {
        TDto GetById(TEntityKey id);
        IEnumerable<TDto> GetAll();        
    }
}