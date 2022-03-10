using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(KokaarQrCoder.Areas.Identity.IdentityHostingStartup))]
namespace KokaarQrCoder.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}