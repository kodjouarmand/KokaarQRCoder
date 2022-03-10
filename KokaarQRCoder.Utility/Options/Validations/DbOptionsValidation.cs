using System.Text;
using KokaarQrCoder.Utility.Enum;
using Microsoft.Extensions.Options;

namespace KokaarQrCoder.Utility.Options.Validations
{
    public class DbOptionsValidation : IValidateOptions<DbOptions>
    {
        public ValidateOptionsResult Validate(string name, DbOptions settings)
        {
            if (string.IsNullOrWhiteSpace(settings.ServerType)
                ||settings.ServerType != DBServerTypeEnum.Sqlite.ToString()  
                || settings.ServerType != DBServerTypeEnum.SqlServer.ToString())
            {
                return ValidateOptionsResult.Fail("\"ServerType\" setting is required in the Db settings of configuration file, and should be" +
                    "of \"Sqlite\" or \"SqlServer\".");
            }
            if (settings.ServerType == DBServerTypeEnum.SqlServer.ToString()
                && string.IsNullOrWhiteSpace(settings.SqlServerConnectionStringName))
            {
                return ValidateOptionsResult.Fail("\"SqlServerConnectionStringName\" setting is required in the Db settings of configuration file.");
            }

            if (settings.ServerType == DBServerTypeEnum.Sqlite.ToString()
                 && string.IsNullOrWhiteSpace(settings.SqliteConnectionStringName))
            {
                return ValidateOptionsResult.Fail("\"SqlServerConnectionStringName\" setting is required in the Db settings of configuration file.");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
