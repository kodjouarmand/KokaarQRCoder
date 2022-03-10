using KokaarQrCoder.Utility.Options;
using Microsoft.Extensions.Options;

namespace KokaarQrCoder.API.Initializer
{
    public interface IDbInitializer
    {
        void Initialize();
    }
}