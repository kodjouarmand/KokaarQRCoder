using Microsoft.Extensions.DependencyInjection;
using KokaarQrCoder.DataAccess.Repositories;
using KokaarQrCoder.DataAccess.Repositories.Contracts;

namespace KokaarQrCoder.Infrastructure
{
    public static class DbServiceExtention
    {        
        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
