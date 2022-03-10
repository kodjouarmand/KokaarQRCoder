using System;
using System.Diagnostics;
using System.Net;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.Domain.ViewModels;
using KokaarQrCoder.Infrastructure.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KokaarQrCoder.Mvc.Controllers
{
    public class HomeController : BaseUserController
    {
        public HomeController(ILoggerService logger, IApplicationUserQuery applicationUserQuery)
            : base(logger, applicationUserQuery)
        {
        }

        public IActionResult Index()
        {
            if (CurrentUser == null)
                return Redirect("/Identity/Account/Login");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var error = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            var contextFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                _logger.LogError(contextFeature.Error);
                error.Message = contextFeature.Error.Message;
            }
            
            return View(error);
        }
    }
}
