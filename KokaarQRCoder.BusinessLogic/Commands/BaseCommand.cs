using AutoMapper;
using KokaarQrCoder.Domain.Entities;
using KokaarQrCoder.Domain.Assemblers;
using System;
using System.Text;
using KokaarQrCoder.BusinessLogic.Exceptions;
using KokaarQrCoder.BusinessLogic.Enums;
using KokaarQrCoder.DataAccess.Repositories.Contracts;
using KokaarQrCoder.BusinessLogic.Commands.Contracts;

namespace KokaarQrCoder.BusinessLogic.Commands
{
    public abstract class BaseCommand<TDto, TEntity, TEntityKey> : IBaseCommand<TDto, TEntityKey> where TDto : BaseDto<TEntityKey> where TEntity : BaseEntity<TEntityKey>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        public DataBaseActionEnum DataBaseAction { get; set; }
        public string CurrentUser { get; set; }

        public BaseCommand(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            DataBaseAction = DataBaseActionEnum.Save;
        }

        public abstract TEntityKey Add(TDto dto);
        public abstract void Update(TDto dto);
        public abstract void Delete(TEntityKey dtoId);
        public abstract void Save();

        protected TEntity BuildEntity(TDto dto)
        {
            StringBuilder validationErrors = new();

            if (string.IsNullOrWhiteSpace(CurrentUser))
            {
                validationErrors.Append("L'utilisateur qui effectue l'opération est requis.");
                throw new CommandValidationException(validationErrors.ToString());
            }
            if (DataBaseAction != DataBaseActionEnum.Save)
            {
                validationErrors.Append("DataBaseAction n'est pas mis à Save.");
                throw new CommandValidationException(validationErrors.ToString());
            }
            if (dto.IsNew())
            {
                validationErrors = ValidateAdd(dto);
            }
            else
            {
                validationErrors = ValidateUpdate(dto);
            }

            if (validationErrors.Length != 0)
            {
                throw new CommandValidationException(validationErrors.ToString());
            }

            TEntity entity = MapDtoToEntity(dto);
            return entity;
        }

        protected abstract StringBuilder ValidateAdd(TDto dto);

        protected abstract StringBuilder ValidateUpdate(TDto dto);

        protected abstract StringBuilder ValidateDelete(TDto dto);

        protected TEntity MapDtoToEntity(TDto dto)
        {
            TEntity entity = _mapper.Map<TEntity>(dto);

            if (dto.IsNew())
            {
                entity.CreationDate = DateTime.Now;
                entity.CreationUser = CurrentUser;
            }
            else
            {                
                entity.LastModificationDate = DateTime.Now;
                entity.LastModificationUser = CurrentUser;
            }

            return entity;
        }
    }
}
