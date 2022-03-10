using System;
using System.Linq;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.Domain.Contexts;
using KokaarQrCoder.Domain.Entities;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.Utility.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KokaarQrCoder.Mvc.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        private readonly ApplicationDbContext _dbContext;

        public UserController(ILoggerService logger, IApplicationUserQuery applicationUserQuery,
            ApplicationDbContext dbContext)
            : base(logger, applicationUserQuery)
        {
            this._dbContext = dbContext;
        }

        public IActionResult Index()
        {
            ViewBag.CurrentUser = CurrentUser;
            return View();
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _dbContext.ApplicationUsers.Include(u => u.Company).ToList();
            var userRole = _dbContext.UserRoles.ToList();
            var roles = _dbContext.Roles.ToList();
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.RoleName = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = "No company"
                    };
                }
            }
            if (!_applicationUserQuery.IsSuperAdministrator(CurrentUser))
                userList.RemoveAll(u => u.RoleName.Equals(StaticHelper.ROLE_NAME_SUPER_ADMIN) 
                || u.CompanyId != CurrentUser.CompanyId);

            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody] Guid id)
        {
            var objFromDb = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                //user is currently locked, we will unlock them
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _dbContext.SaveChanges();
            return Json(new { success = true, message = "Operation Successful." });
        }

        #endregion
    }
}