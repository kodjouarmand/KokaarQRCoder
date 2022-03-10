using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KokaarQrCoder.BusinessLogic.Queries.Contracts
{
    public interface IAgentQuery : IBaseQuery<AgentDto, Guid>
    {
        void GenerateQRCode(AgentDto agent, CompanyDto company, string path = null, bool save = true);
        void GetAgentPayload(AgentDto agent, CompanyDto company, ref StringBuilder payload);
        IEnumerable<AgentDto> GetAll(ApplicationUser user);
        IEnumerable<AgentDto> GetByCompanyId(Guid companyId);
        AgentDto GetByNumber(string number);
    }
}