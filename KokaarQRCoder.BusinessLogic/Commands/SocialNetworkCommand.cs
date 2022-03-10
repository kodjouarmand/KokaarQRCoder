using AutoMapper;
using KokaarQrCoder.Domain.Entities;
using KokaarQrCoder.BusinessLogic.Exceptions;
using System;
using System.Text;
using KokaarQrCoder.BusinessLogic.Enums;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.DataAccess.Repositories.Contracts;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;

namespace KokaarQrCoder.BusinessLogic.Commands.Contracts
{
    public class SocialNetworkCommand : BaseCommand<SocialNetworkDto, SocialNetwork, Guid>, ISocialNetworkCommand
    {

        public SocialNetworkCommand(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        protected override StringBuilder ValidateAdd(SocialNetworkDto socialNetworkDto)
        {
            StringBuilder validationErrors = new();

            if (!socialNetworkDto.IsNew())
            {
                validationErrors.Append("La ressource que vous souhaitez ajouter existe déjà.");
                return validationErrors;
            }
            var validationResult = new SocialNetworkValidator().Validate(socialNetworkDto);
            validationErrors.Append(validationResult.ToString());

            return validationErrors;
        }

        public override Guid Add(SocialNetworkDto socialNetworkDto)
        {
            var socialNetwork = BuildEntity(socialNetworkDto);
            socialNetworkDto.Id = Guid.NewGuid();
            _unitOfWork.SocialNetwork.Add(socialNetwork);
            return socialNetwork.Id;
        }

        protected override StringBuilder ValidateUpdate(SocialNetworkDto socialNetworkDto)
        {
            StringBuilder validationErrors = new();

            if (socialNetworkDto.IsNew())
            {
                validationErrors.Append("La ressource que vous souhaitez mettre à jour n'existe pas.");
                return validationErrors;
            }
            var validationResult = new SocialNetworkValidator().Validate(socialNetworkDto);
            validationErrors.Append(validationResult.ToString());

            return validationErrors;
        }

        public override void Update(SocialNetworkDto socialNetworkDto)
        {
            var socialNetwork = BuildEntity(socialNetworkDto);
            _unitOfWork.SocialNetwork.Update(socialNetwork);
        }

        protected override StringBuilder ValidateDelete(SocialNetworkDto socialNetworkDto = null)
        {
            StringBuilder validationErrors = new();
            if (DataBaseAction != DataBaseActionEnum.Delete)
            {
                validationErrors.Append("DataBaseAction n'est pas mis à Delete.");
            }
            return validationErrors;
        }

        public override void Delete(Guid socialNetworkId)
        {
            var validationErrors = ValidateDelete();
            if (validationErrors.Length == 0)
            {
                _unitOfWork.SocialNetwork.Delete(socialNetworkId);
                return;
            }
            throw new CommandValidationException(validationErrors.ToString());
        }

        public override void Save()
        {
            _unitOfWork.Save();
        }
    }
}
