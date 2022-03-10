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
    public class CompanyQuery : BaseQuery<CompanyDto, Company, Guid>, ICompanyQuery
    {
        private readonly IAgencyQuery _agencyQuery;
        private readonly ISocialNetworkAccountQuery _socialNetworkAccountQuery;
        private readonly IApplicationUserQuery _applicationUserQuery;
        public CompanyQuery(IUnitOfWork unitOfWork, IMapper mapper,
            IAgencyQuery agencyQuery, ISocialNetworkAccountQuery socialNetworkAccountQuery,
            IApplicationUserQuery applicationUserQuery) : base(unitOfWork, mapper) 
        {
            _agencyQuery = agencyQuery;
            _socialNetworkAccountQuery = socialNetworkAccountQuery;
            _applicationUserQuery = applicationUserQuery;
        }

        public override IEnumerable<CompanyDto> GetAll()
        {
            var companies = _unitOfWork.Company.GetAll();           
            return MapEntitiesToDto(companies);
        }

        public override CompanyDto GetById(Guid companyId)
        {
            var company = _unitOfWork.Company.GetAll(c =>  c.Id == companyId)
                .FirstOrDefault();
            return MapEntityToDto(company);
        }

        public IEnumerable<CompanyDto> GetAll(ApplicationUser user)
        {
            if(user == null)
            {
                return new List<CompanyDto>();
            }
            else if (_applicationUserQuery.IsSuperAdministrator(user))
            {
                return GetAll();
            }
            else
            {
                var companies = new List<CompanyDto>
                {
                    GetById(user.CompanyId.GetValueOrDefault())
                };
                return companies;
            }            
        }

        public void GetCompanyPayload(CompanyDto company, ref StringBuilder payload)
        {
            payload.Append($"{company.Name}\n");
            payload.Append($"Tél : {company.PhoneNumber}\n");
            payload.Append($"Email : {company.Email}\n");
            payload.Append($"Web Site : {company.WebSite}\n");
            if (!string.IsNullOrWhiteSpace(company.AdditionnalInformations))
                payload.Append($"Additionl informations : {company.AdditionnalInformations}\n");

            _socialNetworkAccountQuery.GetSocialNetworkAccountPayload(company.Id, ref payload);
            _agencyQuery.GetAgencyPayload(company.Id, ref payload);
        }

        public void GenerateQRCode(CompanyDto company, string path = null, bool save = true)
        {
            StringBuilder payload = new();
            GetCompanyPayload(company, ref payload);
            QRCodeHelper.GenerateQRCode(payload.ToString(), path, save);
        }
    }
}
