using FluentValidation;
using KokaarQrCoder.Domain.Assemblers;

namespace KokaarQrCoder.BusinessLogic.Commands.Contracts
{
    public class SocialNetworkAccountValidator : AbstractValidator<SocialNetworkAccountDto>
    {
        public SocialNetworkAccountValidator()
        {
            RuleFor(socialNetworkAccount => socialNetworkAccount.Account).NotNull().NotEmpty()
                .WithMessage("The name should not be null or empty");
        }
    }
}
