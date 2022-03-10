using AutoMapper;
using KokaarQrCoder.Domain.Entities;
using System;
using System.Collections.Generic;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.DataAccess.Repositories.Contracts;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using System.Linq;
using System.Text;

namespace KokaarQrCoder.BusinessLogic.Queries
{
    public class AgencyQuery : BaseQuery<AgencyDto, Agency, Guid>, IAgencyQuery
    {
        private readonly IApplicationUserQuery _applicationUserQuery;

        public AgencyQuery(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserQuery applicationUserQuery) : base(unitOfWork, mapper) 
        {
            _applicationUserQuery = applicationUserQuery;
        }

        public override IEnumerable<AgencyDto> GetAll()
        {
            var agencies = _unitOfWork.Agency.GetAll(includeProperties: nameof(Company));
            return MapEntitiesToDto(agencies);
        }

        public override AgencyDto GetById(Guid agencyId)
        {
            var agency = _unitOfWork.Agency.GetAll(a => a.Id == agencyId, 
                includeProperties: nameof(Company)).FirstOrDefault();
            return MapEntityToDto(agency);
        }

        public IEnumerable<AgencyDto> GetAll(ApplicationUser user)
        {
            if (user == null)
            {
                return new List<AgencyDto>();
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

        public IEnumerable<AgencyDto> GetByCompanyId(Guid companyId)
        {
            var agencies = _unitOfWork.Agency.GetAll(a => a.CompanyId == companyId,
                includeProperties: $"{nameof(Company)}").ToList();
            return MapEntitiesToDto(agencies);

        }

        public void GetAgencyPayload(Guid companyId, ref StringBuilder payload)
        {
            var agencies = GetByCompanyId(companyId);
            payload.Append('\n');
            foreach (AgencyDto agency in agencies)
            {
                payload.Append($"Agence de {agency.Name.ToUpper()} : {agency.LocationUrl}\n");
            }
        }
    }
}
