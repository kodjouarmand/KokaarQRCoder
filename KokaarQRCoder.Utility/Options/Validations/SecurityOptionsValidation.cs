using System.Text;
using Microsoft.Extensions.Options;

namespace KokaarQrCoder.Utility.Options.Validations
{
    public class SecurityOptionsValidation : IValidateOptions<SecurityOptions>
    {
        public ValidateOptionsResult Validate(string name, SecurityOptions options)
        {
            StringBuilder errors = new();

            if (!int.TryParse(options.RequiredLength.ToString(), out _))
                errors.Append("\"RequiredLength\" should be an integer value;\n");

            if (!bool.TryParse(options.RequireDigit.ToString(), out _))
                errors.Append("\"RequireDigit\" should be a boolean value (true or false);\n");

            if (!bool.TryParse(options.RequireUppercase.ToString(), out _))
                errors.Append("\"RequireUppercase\" should be a boolean value (true or false);\n");

            if (!bool.TryParse(options.RequireLowercase.ToString(), out _))
                errors.Append("\"RequireLowercase\" should be a boolean value (true or false);\n");

            if (!bool.TryParse(options.RequireNonAlphanumeric.ToString(), out _))
                errors.Append("\"RequireNonAlphanumeric\" should be a boolean value (true or false);\n");

            if (!int.TryParse(options.RequiredUniqueChars.ToString(), out _))
                errors.Append("\"RequiredUniqueChars\" should be an integer value;\n");

            if (!bool.TryParse(options.RequireUniqueEmail.ToString(), out _))
                errors.Append("\"RequireUniqueEmail\" should be a boolean value (true or false);\n");

            if (!int.TryParse(options.MaxFailedAccessAttempts.ToString(), out _))
                errors.Append("\"MaxFailedAccessAttempts\" should be aan integer value;\n");

            if (!int.TryParse(options.DefaultLockoutTimeSpan.ToString(), out _))
                errors.Append("\"DefaultLockoutTimeSpan\" should be an integer value;\n");

            if (!bool.TryParse(options.RequireConfirmedAccount.ToString(), out _))
                errors.Append("\"RequireConfirmedAccount\" should be a boolean value (true or false);\n");

            if (!bool.TryParse(options.RequireConfirmedEmail.ToString(), out _))
                errors.Append("\"RequireConfirmedEmail\" should be a boolean value (true or false);\n");

            if (!bool.TryParse(options.RequireConfirmedPhoneNumber.ToString(), out _))
                errors.Append("\"RequireConfirmedPhoneNumber\" should be a boolean value (true or false);\n");


            if (errors.Length != 0)
                errors.Insert(0, "Please check the Identity's options in the configuration file :\n");

            return errors.Length != 0 ? ValidateOptionsResult.Fail(errors.ToString()) : ValidateOptionsResult.Success;
        }
    }
}
