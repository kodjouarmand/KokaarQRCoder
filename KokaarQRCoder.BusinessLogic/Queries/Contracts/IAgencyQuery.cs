using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KokaarQrCoder.BusinessLogic.Queries.Contracts
{
    public interface IAgencyQuery : IBaseQuery<AgencyDto, Guid>
    {
        IEnumerable<AgencyDto> GetByCompanyId(Guid companyId);
        void GetAgencyPayload(Guid companyId, ref StringBuilder payload);
        IEnumerable<AgencyDto> GetAll(ApplicationUser user);
    }
}