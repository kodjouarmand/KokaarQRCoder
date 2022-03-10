using Microsoft.AspNetCore.Mvc;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.BusinessLogic.Enums;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.BusinessLogic.Commands.Contracts;
using System.Linq;
using KokaarQrCoder.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using KokaarQrCoder.Mvc;
using System;
using Microsoft.Extensions.Configuration;

namespace KokaarQrCoder.Areas.Admin.Controllers
{
    public class AgencyController : BaseAdminController
    {
        private readonly IAgencyQuery _agencyQuery;
        private readonly IAgencyCommand _agencyCommand;
        private readonly ICompanyQuery _companyQuery;
        public AgencyController(ILoggerService logger, IApplicationUserQuery applicationUserQuery,
            IAgencyQuery agencyQuery, IAgencyCommand agencyCommand, ICompanyQuery companyQuery)
            : base(logger, applicationUserQuery)
        {
            _agencyQuery = agencyQuery;
            _agencyCommand = agencyCommand;
            _companyQuery = companyQuery;
        }

        public IActionResult Index()
        {
            var agencies = _agencyQuery.GetAll(CurrentUser);
            return View(agencies);
        }

        public IActionResult Upsert(Guid? id)
        {
            AgencyViewModel agencyViewModel = new()
            {
                Agency = new AgencyDto(),
                CompanyList = _companyQuery.GetAll(CurrentUser).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //this is for create
                return View(agencyViewModel);
            }
            //this is for edit
            agencyViewModel.Agency = _agencyQuery.GetById(id.GetValueOrDefault());
            if (agencyViewModel.Agency == null)
            {
                return NotFound();
            }
            return View(agencyViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(AgencyViewModel agencyViewModel)
        {
            var agencyDto = agencyViewModel.Agency;
            if (ModelState.IsValid)
            {
                _agencyCommand.CurrentUser = CurrentUser.UserName;
                if (agencyDto.Id == Guid.Empty)
                {
                    _agencyCommand.Add(agencyDto);
                }
                else
                {
                    _agencyCommand.Update(agencyDto);
                }
                _agencyCommand.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                agencyViewModel.CompanyList = _companyQuery.GetAll(CurrentUser).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if (agencyDto.Id != Guid.Empty)
                {
                    agencyDto = _agencyQuery.GetById(agencyDto.Id);
                }
            }
            return View(agencyViewModel);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var agencies = _agencyQuery.GetAll(CurrentUser);
            return Json(new { data = agencies });
        }

        //[HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var agency = _agencyQuery.GetById(id);
            if (agency == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _agencyCommand.DataBaseAction = DataBaseActionEnum.Delete;
            _agencyCommand.Delete(id);
            _agencyCommand.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }

}