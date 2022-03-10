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
    public class SocialNetworkController : BaseAdminController
    {
        private readonly ISocialNetworkQuery _socialNetworkQuery;
        private readonly ISocialNetworkCommand _socialNetworkCommand;
        public SocialNetworkController(ILoggerService logger, IApplicationUserQuery applicationUserQuery,
            ISocialNetworkQuery socialNetworkQuery, ISocialNetworkCommand socialNetworkCommand)
            : base(logger, applicationUserQuery)
        {
            _socialNetworkQuery = socialNetworkQuery;
            _socialNetworkCommand = socialNetworkCommand;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(Guid? id)
        {
            SocialNetworkDto socialNetwork = new();
            if (id == null)
            {
                //this is for create
                return View(socialNetwork);
            }
            //this is for edit
            socialNetwork = _socialNetworkQuery.GetById(id.GetValueOrDefault());
            if (socialNetwork == null)
            {
                return NotFound();
            }
            return View(socialNetwork);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(SocialNetworkDto socialNetwork)
        {
            if (ModelState.IsValid)
            {
                _socialNetworkCommand.CurrentUser = CurrentUser.UserName;
                if (socialNetwork.Id == Guid.Empty)
                {
                    _socialNetworkCommand.Add(socialNetwork);

                }
                else
                {
                    _socialNetworkCommand.Update(socialNetwork);
                }
                _socialNetworkCommand.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(socialNetwork);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _socialNetworkQuery.GetAll();
            return Json(new { data = companies });
        }

        //[HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var socialNetwork = _socialNetworkQuery.GetById(id);
            if (socialNetwork == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _socialNetworkCommand.DataBaseAction = DataBaseActionEnum.Delete;
            _socialNetworkCommand.Delete(id);
            _socialNetworkCommand.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }

}