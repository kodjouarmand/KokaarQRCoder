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
    public class SocialNetworkAccountCommand : BaseCommand<SocialNetworkAccountDto, SocialNetworkAccount, Guid>, ISocialNetworkAccountCommand
    {

        public SocialNetworkAccountCommand(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        protected override StringBuilder ValidateAdd(SocialNetworkAccountDto socialNetworkAccountDto)
        {
            StringBuilder validationErrors = new();

            if (!socialNetworkAccountDto.IsNew())
            {
                validationErrors.Append("La ressource que vous souhaitez ajouter existe déjà.");
                return validationErrors;
            }
            var validationResult = new SocialNetworkAccountValidator().Validate(socialNetworkAccountDto);
            validationErrors.Append(validationResult.ToString());

            return validationErrors;
        }

        public override Guid Add(SocialNetworkAccountDto socialNetworkAccountDto)
        {
            var socialNetworkAccount = BuildEntity(socialNetworkAccountDto);
            socialNetworkAccountDto.Id = Guid.NewGuid();
            _unitOfWork.SocialNetworkAccount.Add(socialNetworkAccount);
            return socialNetworkAccount.Id;
        }

        protected override StringBuilder ValidateUpdate(SocialNetworkAccountDto socialNetworkAccountDto)
        {
            StringBuilder validationErrors = new();

            if (socialNetworkAccountDto.IsNew())
            {
                validationErrors.Append("La ressource que vous souhaitez mettre à jour n'existe pas.");
                return validationErrors;
            }
            var validationResult = new SocialNetworkAccountValidator().Validate(socialNetworkAccountDto);
            validationErrors.Append(validationResult.ToString());

            return validationErrors;
        }

        public override void Update(SocialNetworkAccountDto socialNetworkAccountDto)
        {
            var socialNetworkAccount = BuildEntity(socialNetworkAccountDto);
            _unitOfWork.SocialNetworkAccount.Update(socialNetworkAccount);
        }

        protected override StringBuilder ValidateDelete(SocialNetworkAccountDto socialNetworkAccountDto = null)
        {
            StringBuilder validationErrors = new();
            if (DataBaseAction != DataBaseActionEnum.Delete)
            {
                validationErrors.Append("DataBaseAction n'est pas mis à Delete.");
            }
            return validationErrors;
        }

        public override void Delete(Guid socialNetworkAccountId)
        {
            var validationErrors = ValidateDelete();
            if (validationErrors.Length == 0)
            {
                _unitOfWork.SocialNetworkAccount.Delete(socialNetworkAccountId);
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
