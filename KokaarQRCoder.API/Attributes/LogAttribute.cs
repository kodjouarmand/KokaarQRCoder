using KokaarQrCoder.Infrastructure.Contracts;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KokaarQrCoder.API.Attributes
{
    public class LogAttribute : IActionFilter
    {
        private readonly ILoggerService _logger;
        public LogAttribute(ILoggerService logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }

        public void OnActionExecuted(ActionExecutedContext context) 
        { 

        }
    }

}
