using KokaarQrCoder.Domain.Assemblers;
using System.Collections.Generic;
using KokaarQrCoder.BusinessLogic.Enums;

namespace KokaarQrCoder.BusinessLogic.Commands.Contracts
{
    public interface IBaseCommand<TDto, TEntityKey> where TDto : BaseDto<TEntityKey>
    {
        DataBaseActionEnum DataBaseAction { get; set; }
        string CurrentUser { get; set; }

        TEntityKey Add(TDto dto);
        void Update(TDto dto);
        void Delete(TEntityKey dtoId);
        void Save();
    }
}