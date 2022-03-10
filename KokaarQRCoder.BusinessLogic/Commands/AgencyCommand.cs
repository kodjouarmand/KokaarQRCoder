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
    public class AgencyCommand : BaseCommand<AgencyDto, Agency, Guid>, IAgencyCommand
    {

        public AgencyCommand(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        protected override StringBuilder ValidateAdd(AgencyDto agencyDto)
        {
            StringBuilder validationErrors = new();

            if (!agencyDto.IsNew())
            {
                validationErrors.Append("La ressource que vous souhaitez ajouter existe déjà.");
                return validationErrors;
            }
            var validationResult = new AgencyValidator().Validate(agencyDto);
            validationErrors.Append(validationResult.ToString());

            return validationErrors;
        }

        public override Guid Add(AgencyDto agencyDto)
        {
            var agency = BuildEntity(agencyDto);
            agencyDto.Id = Guid.NewGuid();
            _unitOfWork.Agency.Add(agency);
            return agency.Id;
        }

        protected override StringBuilder ValidateUpdate(AgencyDto agencyDto)
        {
            StringBuilder validationErrors = new();

            if (agencyDto.IsNew())
            {
                validationErrors.Append("La ressource que vous souhaitez mettre à jour n'existe pas.");
                return validationErrors;
            }
            var validationResult = new AgencyValidator().Validate(agencyDto);
            validationErrors.Append(validationResult.ToString());

            return validationErrors;
        }

        public override void Update(AgencyDto agencyDto)
        {
            var agency = BuildEntity(agencyDto);
            _unitOfWork.Agency.Update(agency);
        }

        protected override StringBuilder ValidateDelete(AgencyDto agencyDto = null)
        {
            StringBuilder validationErrors = new();
            if (DataBaseAction != DataBaseActionEnum.Delete)
            {
                validationErrors.Append("DataBaseAction n'est pas mis à Delete.");
            }
            return validationErrors;
        }

        public override void Delete(Guid agencyId)
        {
            var validationErrors = ValidateDelete();
            if (validationErrors.Length == 0)
            {
                _unitOfWork.Agency.Delete(agencyId);
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
