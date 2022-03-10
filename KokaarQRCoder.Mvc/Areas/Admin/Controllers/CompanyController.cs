using Microsoft.AspNetCore.Mvc;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.BusinessLogic.Enums;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.BusinessLogic.Commands.Contracts;
using KokaarQrCoder.Mvc;
using System;
using Microsoft.Extensions.Configuration;

namespace KokaarQrCoder.Areas.Admin.Controllers
{
    public class CompanyController : BaseAdminController
    {
        private readonly ICompanyQuery _companyQuery;
        private readonly ICompanyCommand _companyCommand;
        public CompanyController(ILoggerService logger, IApplicationUserQuery applicationUserQuery,
            ICompanyQuery companyQuery, ICompanyCommand companyCommand)
            : base(logger, applicationUserQuery)
        {            
            _companyQuery = companyQuery;
            _companyCommand = companyCommand;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(Guid? id)
        {
            CompanyDto company = new();
            if (id == null)
            {
                //this is for create
                return View(company);
            }
            //this is for edit
            company = _companyQuery.GetById(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }
            return View(company);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CompanyDto company)
        {
            if (ModelState.IsValid)
            {
                _companyCommand.CurrentUser = CurrentUser.UserName;
                if (company.Id == Guid.Empty)
                {
                    _companyCommand.Add(company);

                }
                else
                {
                    _companyCommand.Update(company);
                }
                _companyCommand.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _companyQuery.GetAll(CurrentUser);
            return Json(new { data = companies });
        }

        //[HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var company = _companyQuery.GetById(id);
            if (company == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _companyCommand.DataBaseAction = DataBaseActionEnum.Delete;
            _companyCommand.Delete(id);
            _companyCommand.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }

}