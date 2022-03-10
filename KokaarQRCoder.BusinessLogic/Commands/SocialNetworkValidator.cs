using FluentValidation;
using KokaarQrCoder.Domain.Assemblers;

namespace KokaarQrCoder.BusinessLogic.Commands.Contracts
{
    public class SocialNetworkValidator : AbstractValidator<SocialNetworkDto>
    {
        public SocialNetworkValidator()
        {
            RuleFor(socialNetwork => socialNetwork.Name).NotNull().NotEmpty()
                .WithMessage("The name should not be null or empty");
        }
    }
}
