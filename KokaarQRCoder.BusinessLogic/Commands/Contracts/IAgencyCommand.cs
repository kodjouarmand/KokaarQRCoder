using KokaarQrCoder.Domain.Assemblers;
using System;

namespace KokaarQrCoder.BusinessLogic.Commands.Contracts
{
    public interface IAgencyCommand : IBaseCommand<AgencyDto, Guid>
    {

    }
}