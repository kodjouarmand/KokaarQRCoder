using FluentValidation;
using KokaarQrCoder.Domain.Assemblers;

namespace KokaarQrCoder.BusinessLogic.Commands.Contracts
{
    public class AgencyValidator : AbstractValidator<AgencyDto>
    {
        public AgencyValidator()
        {
            RuleFor(agency => agency.LocationUrl).NotNull().NotEmpty()
                .WithMessage("The locationURL should not be null or empty");
        }
    }
}
