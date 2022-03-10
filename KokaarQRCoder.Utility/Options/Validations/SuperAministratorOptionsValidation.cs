using System.Text;
using KokaarQrCoder.Utility.Enum;
using Microsoft.Extensions.Options;

namespace KokaarQrCoder.Utility.Options.Validations
{
    public class SuperAministratorOptionsValidation : IValidateOptions<SuperAministratorOptions>
    {
        public ValidateOptionsResult Validate(string name, SuperAministratorOptions settings)
        {
            StringBuilder errors = new();
            if (string.IsNullOrWhiteSpace(settings.UserId))
                errors.Append("\"UserId\" is required;\n");
            if (string.IsNullOrWhiteSpace(settings.UserName))
                errors.Append("\"UserName\" is required;\n");
            if (string.IsNullOrWhiteSpace(settings.Password))
                errors.Append("\"Password\" is required;\n");
            if (string.IsNullOrWhiteSpace(settings.Name))
                errors.Append("\"Name\" is required;\n");
            if (string.IsNullOrWhiteSpace(settings.Email))
                errors.Append("\"Email\" is required;\n");
            if (string.IsNullOrWhiteSpace(settings.RoleId))
                errors.Append("\"RoleId\" is required;\n");
            if (string.IsNullOrWhiteSpace(settings.RoleName))
                errors.Append("\"RoleName\" is required;\n");

            if (errors.Length != 0)
                errors.Insert(0, "Please check the Super Aministrator's settings of configuration file :\n");

            return errors.Length != 0 ? ValidateOptionsResult.Fail(errors.ToString()) : ValidateOptionsResult.Success;
        }
    }
}
