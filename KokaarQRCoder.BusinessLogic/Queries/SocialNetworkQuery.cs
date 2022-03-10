using AutoMapper;
using KokaarQrCoder.Domain.Entities;
using System;
using System.Collections.Generic;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.DataAccess.Repositories.Contracts;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;

namespace KokaarQrCoder.BusinessLogic.Queries
{
    public class SocialNetworkQuery : BaseQuery<SocialNetworkDto, SocialNetwork, Guid>, ISocialNetworkQuery
    {
        public SocialNetworkQuery(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public override IEnumerable<SocialNetworkDto> GetAll()
        {
            var companies = _unitOfWork.SocialNetwork.GetAll();
            return MapEntitiesToDto(companies);
        }

        public override SocialNetworkDto GetById(Guid socialNetworkId)
        {
            var socialNetwork = _unitOfWork.SocialNetwork.GetById(socialNetworkId);
            return MapEntityToDto(socialNetwork);
        }
    }
}
