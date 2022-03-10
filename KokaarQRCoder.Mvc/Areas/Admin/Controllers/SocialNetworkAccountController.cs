using Microsoft.AspNetCore.Mvc;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.BusinessLogic.Enums;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.BusinessLogic.Commands.Contracts;
using KokaarQrCoder.Domain.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using KokaarQrCoder.Mvc;
using System;
using Microsoft.Extensions.Configuration;

namespace KokaarQrCoder.Areas.Admin.Controllers
{
    public class SocialNetworkAccountController : BaseAdminController
    {
        private readonly ISocialNetworkAccountQuery _socialNetworkAccountQuery;
        private readonly ISocialNetworkAccountCommand _socialNetworkAccountCommand;
        private readonly ISocialNetworkQuery _socialNetworkQuery;
        private readonly ICompanyQuery _companyQuery;

        public SocialNetworkAccountController(ILoggerService logger, IApplicationUserQuery applicationUserQuery,
            ISocialNetworkAccountQuery socialNetworkAccountQuery, ISocialNetworkAccountCommand socialNetworkAccountCommand,
            ISocialNetworkQuery socialNetworkQuery, ICompanyQuery companyQuery)
            : base(logger, applicationUserQuery)
        {
            _socialNetworkAccountQuery = socialNetworkAccountQuery;
            _socialNetworkAccountCommand = socialNetworkAccountCommand;
            _socialNetworkQuery = socialNetworkQuery;
            _companyQuery = companyQuery;
        }

        public IActionResult Index()
        {
            var socialNetworkAccounts = _socialNetworkAccountQuery.GetAll(CurrentUser);
            return View(socialNetworkAccounts);
        }

        public IActionResult Upsert(Guid? id)
        {
            SocialNetworkAccountViewModel socialNetworkAccountViewModel = new()
            {
                SocialNetworkAccount = new SocialNetworkAccountDto(),
                SocialNetworkList = _socialNetworkQuery.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CompanyList = _companyQuery.GetAll(CurrentUser).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //this is for create
                return View(socialNetworkAccountViewModel);
            }
            //this is for edit
            socialNetworkAccountViewModel.SocialNetworkAccount = _socialNetworkAccountQuery.GetById(id.GetValueOrDefault());
            if (socialNetworkAccountViewModel.SocialNetworkAccount == null)
            {
                return NotFound();
            }
            return View(socialNetworkAccountViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(SocialNetworkAccountViewModel socialNetworkAccountViewModel)
        {
            var socialNetworkAccountDto = socialNetworkAccountViewModel.SocialNetworkAccount;
            if (ModelState.IsValid)
            {
                _socialNetworkAccountCommand.CurrentUser = CurrentUser.UserName;
                if (socialNetworkAccountDto.Id == Guid.Empty)
                {
                    _socialNetworkAccountCommand.Add(socialNetworkAccountDto);
                }
                else
                {
                    _socialNetworkAccountCommand.Update(socialNetworkAccountDto);
                }
                _socialNetworkAccountCommand.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                socialNetworkAccountViewModel.SocialNetworkList = _socialNetworkQuery.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                socialNetworkAccountViewModel.CompanyList = _companyQuery.GetAll(CurrentUser).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if (socialNetworkAccountDto.Id != Guid.Empty)
                {
                    socialNetworkAccountDto = _socialNetworkAccountQuery.GetById(socialNetworkAccountDto.Id);
                }
            }
            return View(socialNetworkAccountViewModel);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var socialNetworkAccounts = _socialNetworkAccountQuery.GetAll(CurrentUser);
            return Json(new { data = socialNetworkAccounts });
        }

        //[HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var socialNetworkAccount = _socialNetworkAccountQuery.GetById(id);
            if (socialNetworkAccount == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _socialNetworkAccountCommand.DataBaseAction = DataBaseActionEnum.Delete;
            _socialNetworkAccountCommand.Delete(id);
            _socialNetworkAccountCommand.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }

}