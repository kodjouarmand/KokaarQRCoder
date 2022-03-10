using FluentValidation;
using KokaarQrCoder.Domain.Assemblers;

namespace KokaarQrCoder.BusinessLogic.Commands.Contracts
{
    public class CompanyValidator : AbstractValidator<CompanyDto>
    {
        public CompanyValidator()
        {
            RuleFor(company => company.Name).NotNull().NotEmpty()
                .WithMessage("The name should not be null or empty");
        }
    }
}
