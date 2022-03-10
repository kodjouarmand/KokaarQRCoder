using System.Security.Claims;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.Domain.Entities;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.Utility.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace KokaarQrCoder.Mvc.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected readonly ILoggerService _logger;
        protected readonly IApplicationUserQuery _applicationUserQuery;
        public BaseController(ILoggerService logger, IApplicationUserQuery applicationUserQuery)
        {
            _logger = logger;
            _applicationUserQuery = applicationUserQuery;
        }

        public ApplicationUser CurrentUser
        {
            get => _applicationUserQuery.GetCurrentUser((ClaimsIdentity)User.Identity);
        }
    }
}
