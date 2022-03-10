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
    public class SocialNetworkAccountQuery : BaseQuery<SocialNetworkAccountDto, SocialNetworkAccount, Guid>, ISocialNetworkAccountQuery
    {
        private readonly IApplicationUserQuery _applicationUserQuery;
        public SocialNetworkAccountQuery(IUnitOfWork unitOfWork, IMapper mapper, IApplicationUserQuery applicationUserQuery) : base(unitOfWork, mapper)
        {
            _applicationUserQuery = applicationUserQuery;
        }

        public override IEnumerable<SocialNetworkAccountDto> GetAll()
        {
            var socialNetworkAccounts = _unitOfWork.SocialNetworkAccount
                .GetAll(includeProperties: $"{nameof(Company)},{nameof(SocialNetwork)}");
            return MapEntitiesToDto(socialNetworkAccounts);
        }

        public override SocialNetworkAccountDto GetById(Guid socialNetworkAccountId)
        {
            var socialNetworkAccount = _unitOfWork.SocialNetworkAccount.GetAll(s => s.Id == socialNetworkAccountId,
                includeProperties: $"{nameof(Company)},{nameof(SocialNetwork)}").ToList().FirstOrDefault();
            return MapEntityToDto(socialNetworkAccount);
        }

        public IEnumerable<SocialNetworkAccountDto> GetAll(ApplicationUser user)
        {
            if (user == null)
            {
                return new List<SocialNetworkAccountDto>();
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
        public IEnumerable<SocialNetworkAccountDto> GetByCompanyId(Guid companyId)
        {
            var socialNetworkAccounts = _unitOfWork.SocialNetworkAccount.GetAll(s => s.CompanyId == companyId,
                includeProperties: $"{nameof(Company)},{nameof(SocialNetwork)}").ToList();
            return MapEntitiesToDto(socialNetworkAccounts);

        }
        public void GetSocialNetworkAccountPayload(Guid companyId, ref StringBuilder payload)
        {
            var socialNetworkAccounts = GetByCompanyId(companyId);
            foreach (SocialNetworkAccountDto socialNetworkAccount in socialNetworkAccounts)
            {
                payload.Append($"{socialNetworkAccount.SocialNetwork.Name} : {socialNetworkAccount.Account}\n");
            }
        }
    }
}
