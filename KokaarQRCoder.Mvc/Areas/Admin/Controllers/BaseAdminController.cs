using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.Mvc.Controllers;
using KokaarQrCoder.Utility.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KokaarQrCoder.Mvc
{
    [Area("Admin")]
    [Authorize(Roles = StaticHelper.ROLE_NAME_SUPER_ADMIN + ", " + StaticHelper.ROLE_NAME_ADMIN)]
    public class BaseAdminController : BaseController
    {
        public BaseAdminController(ILoggerService logger, IApplicationUserQuery applicationUserQuery)
            : base(logger, applicationUserQuery) { }
    }
}
