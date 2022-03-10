using FluentValidation;
using KokaarQrCoder.Domain.Assemblers;

namespace KokaarQrCoder.BusinessLogic.Commands.Contracts
{
    public class AgentValidator : AbstractValidator<AgentDto>
    {
        public AgentValidator()
        {
            RuleFor(agent => agent.Name).NotNull().NotEmpty()
                .WithMessage("The name should not be null or empty");
        }
    }
}
