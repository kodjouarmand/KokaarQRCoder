using System.Text;
using Microsoft.Extensions.Options;

namespace KokaarQrCoder.Utility.Options.Validations
{
    public class EmailOptionsValidation : IValidateOptions<EmailOptions>
    {
        public ValidateOptionsResult Validate(string name, EmailOptions options)
        {
            if (!bool.TryParse(options.WillSendEmail.ToString(), out bool willSendEmail))
                return ValidateOptionsResult.Fail("\"WillSendEmail\" setting is required in the Email settings of configuration file.");

            StringBuilder errors = new();
            if (willSendEmail)
            {
                if (string.IsNullOrWhiteSpace(options.SmtpServer))
                    errors.Append("\"SmtpServer\" is required;\n");
                if (string.IsNullOrWhiteSpace(options.Sender))
                    errors.Append("\"Sender\" is required;\n");
                if (options.Port == null)
                    errors.Append("\"Port\" is required;\n");
                if (string.IsNullOrWhiteSpace(options.UserName))
                    errors.Append("\"UserName\" is required;\n");
                if (string.IsNullOrWhiteSpace(options.Password))
                    errors.Append("\"Password\" is required;\n");
            }

            if (errors.Length != 0)
                errors.Insert(0, "Please check the Super Aministrator's settings of configuration file :\n");

            return errors.Length != 0 ? ValidateOptionsResult.Fail(errors.ToString()) : ValidateOptionsResult.Success;
        }
    }
}
