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
    public class CompanyCommand : BaseCommand<CompanyDto, Company, Guid>, ICompanyCommand
    {

        public CompanyCommand(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        protected override StringBuilder ValidateAdd(CompanyDto companyDto)
        {
            StringBuilder validationErrors = new();

            if (!companyDto.IsNew())
            {
                validationErrors.Append("La ressource que vous souhaitez ajouter existe déjà.");
                return validationErrors;
            }
            var validationResult = new CompanyValidator().Validate(companyDto);
            validationErrors.Append(validationResult.ToString());

            return validationErrors;
        }

        public override Guid Add(CompanyDto companyDto)
        {
            var company = BuildEntity(companyDto);
            companyDto.Id = Guid.NewGuid();
            _unitOfWork.Company.Add(company);
            return company.Id;
        }

        protected override StringBuilder ValidateUpdate(CompanyDto companyDto)
        {
            StringBuilder validationErrors = new();

            if (companyDto.IsNew())
            {
                validationErrors.Append("La ressource que vous souhaitez mettre à jour n'existe pas.");
                return validationErrors;
            }
            var validationResult = new CompanyValidator().Validate(companyDto);
            validationErrors.Append(validationResult.ToString());

            return validationErrors;
        }

        public override void Update(CompanyDto companyDto)
        {
            var company = BuildEntity(companyDto);
            _unitOfWork.Company.Update(company);
        }

        protected override StringBuilder ValidateDelete(CompanyDto companyDto = null)
        {
            StringBuilder validationErrors = new();
            if (DataBaseAction != DataBaseActionEnum.Delete)
            {
                validationErrors.Append("DataBaseAction n'est pas mis à Delete.");
            }
            return validationErrors;
        }

        public override void Delete(Guid companyId)
        {
            var validationErrors = ValidateDelete();
            if (validationErrors.Length == 0)
            {
                _unitOfWork.Company.Delete(companyId);
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
