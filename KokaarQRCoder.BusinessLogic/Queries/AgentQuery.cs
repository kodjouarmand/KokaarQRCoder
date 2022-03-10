using AutoMapper;
using KokaarQrCoder.Domain.Entities;
using System.Collections.Generic;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.DataAccess.Repositories.Contracts;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using System.Linq;
using System.Text;
using KokaarQrCoder.Utility.Helpers;
using System;

namespace KokaarQrCoder.BusinessLogic.Queries
{
    public class AgentQuery : BaseQuery<AgentDto, Agent, Guid>, IAgentQuery
    {
        private readonly ICompanyQuery _companyQuery;
        private readonly IApplicationUserQuery _applicationUserQuery;

        public AgentQuery(IUnitOfWork unitOfWork, IMapper mapper, ICompanyQuery companyQuery,
            IApplicationUserQuery applicationUserQuery) : base(unitOfWork, mapper)
        {
            _companyQuery = companyQuery;
            _applicationUserQuery = applicationUserQuery;
        }

        public override IEnumerable<AgentDto> GetAll()
        {
            var agents = _unitOfWork.Agent.GetAll(includeProperties: nameof(Company));
            return MapEntitiesToDto(agents);
        }

        public override AgentDto GetById(Guid agentId)
        {
            var agent = _unitOfWork.Agent.GetAll(a => a.Id == agentId,
                includeProperties: nameof(Company)).FirstOrDefault();
            return MapEntityToDto(agent);
        }

        public IEnumerable<AgentDto> GetAll(ApplicationUser user)
        {
            if (user == null)
            {
                return new List<AgentDto>();
            }
            else if (_applicationUserQuery.IsSuperAdministrator(user))
            {
                return GetAll();
            }
            else
            {
                return GetByCompanyId(user.CompanyId.GetValueOrDefault());
            }
        }

        public AgentDto GetByNumber(string number)
        {
            var agent = _unitOfWork.Agent.GetAll(a => a.Number == number,
                includeProperties: nameof(Company)).FirstOrDefault();
            return MapEntityToDto(agent);
        }

        public IEnumerable<AgentDto> GetByCompanyId(Guid companyId)
        {
            var agents = _unitOfWork.Agent.GetAll(a => a.CompanyId == companyId,
                includeProperties: $"{nameof(Company)}").ToList();
            return MapEntitiesToDto(agents);

        }

        public void GetAgentPayload(AgentDto agent, CompanyDto company, ref StringBuilder payload)
        {
            payload.Append($"{agent.Name}\n");
            payload.Append($"Tél : {agent.PhoneNumber}\n");
            payload.Append($"Email : {agent.Email}\n");
            payload.Append($"---------\n");
            _companyQuery.GetCompanyPayload(company, ref payload);
        }

        public void GenerateQRCode(AgentDto agent, CompanyDto company, string path = null, bool save = true)
        {
            StringBuilder payload = new();
            GetAgentPayload(agent, company, ref payload);
            QRCodeHelper.GenerateQRCode(payload.ToString(), path, save);
        }
    }
}
