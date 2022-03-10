using System;
using KokaarQrCoder.API.Initializer;
using KokaarQrCoder.Domain.Contexts;
using KokaarQrCoder.Domain.Entities;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.Utility.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace KokaarQrCoder.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DbInitializerController : ControllerBase
    {

        private readonly IDbInitializer _dbInitializer;
        public DbInitializerController(IDbInitializer dbInitializer)
        {
            _dbInitializer = dbInitializer;
        }

        [HttpPost]
        public IActionResult Post()
        {
            try
            {
                _dbInitializer.Initialize();
                return Ok("Database initialization succeed");
            }
            catch (Exception)
            {
                return BadRequest("Database initialization failed. Please checks the log files for more information about the error.");
            }
        }
    }
}
