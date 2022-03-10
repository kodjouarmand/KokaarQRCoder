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
    public class AgentCommand : BaseCommand<AgentDto, Agent, Guid>, IAgentCommand
    {
        private readonly IAgentQuery _agentQuery;
        public AgentCommand(IUnitOfWork unitOfWork, IMapper mapper, IAgentQuery agentQuery) : base(unitOfWork, mapper)
        {
            _agentQuery = agentQuery;
        }

        protected override StringBuilder ValidateAdd(AgentDto agentDto)
        {
            StringBuilder validationErrors = new();

            if (!agentDto.IsNew() || _agentQuery.GetByNumber(agentDto.Number) != null)
            {
                validationErrors.Append("La ressource que vous souhaitez ajouter existe déjà.");
                return validationErrors;
            }
            var validationResult = new AgentValidator().Validate(agentDto);
            validationErrors.Append(validationResult.ToString());

            return validationErrors;
        }

        public override Guid Add(AgentDto agentDto)
        {
            var agent = BuildEntity(agentDto);
            agentDto.Id = Guid.NewGuid();
            _unitOfWork.Agent.Add(agent);
            return agent.Id;
        }

        protected override StringBuilder ValidateUpdate(AgentDto agentDto)
        {
            StringBuilder validationErrors = new();

            if (agentDto.IsNew())
            {
                validationErrors.Append("La ressource que vous souhaitez mettre à jour n'existe pas.");
                return validationErrors;
            }
            var validationResult = new AgentValidator().Validate(agentDto);
            validationErrors.Append(validationResult.ToString());

            return validationErrors;
        }

        public override void Update(AgentDto agentDto)
        {
            var agent = BuildEntity(agentDto);
            _unitOfWork.Agent.Update(agent);
        }

        protected override StringBuilder ValidateDelete(AgentDto agentDto = null)
        {
            StringBuilder validationErrors = new();
            if (DataBaseAction != DataBaseActionEnum.Delete)
            {
                validationErrors.Append("DataBaseAction n'est pas mis à Delete.");
            }
            return validationErrors;
        }

        public override void Delete(Guid agentId)
        {
            var validationErrors = ValidateDelete();
            if (validationErrors.Length == 0)
            {
                _unitOfWork.Agent.Delete(agentId);
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
