using KokaarQrCoder.BusinessLogic.Queries;
using KokaarQrCoder.Domain.Assemblers;
using System;

namespace KokaarQrCoder.BusinessLogic.Commands.Contracts
{
    public interface ISocialNetworkCommand : IBaseCommand<SocialNetworkDto, Guid>
    {

    }
}