using KokaarQrCoder.BusinessLogic.Queries;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KokaarQrCoder.BusinessLogic.Queries.Contracts
{
    public interface ISocialNetworkAccountQuery : IBaseQuery<SocialNetworkAccountDto, Guid>
    {
        IEnumerable<SocialNetworkAccountDto> GetAll(ApplicationUser user);
        IEnumerable<SocialNetworkAccountDto> GetByCompanyId(Guid companyId);
        void GetSocialNetworkAccountPayload(Guid companyId, ref StringBuilder payload);
    }
}