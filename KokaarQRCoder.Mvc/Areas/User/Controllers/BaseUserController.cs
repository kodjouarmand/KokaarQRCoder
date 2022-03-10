using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KokaarQrCoder.Mvc
{
    [Area("User")]
    public class BaseUserController : BaseController
    {
        public BaseUserController(ILoggerService logger, IApplicationUserQuery applicationUserQuery)
            : base(logger, applicationUserQuery) { }
    }
}
