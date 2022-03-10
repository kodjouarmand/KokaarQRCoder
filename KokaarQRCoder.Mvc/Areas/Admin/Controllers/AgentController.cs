using Microsoft.AspNetCore.Mvc;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.BusinessLogic.Enums;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.BusinessLogic.Commands.Contracts;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using KokaarQrCoder.Utility.Helpers;
using KokaarQrCoder.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using KokaarQrCoder.Mvc;
using Microsoft.Extensions.Configuration;

namespace KokaarQrCoder.Areas.Admin.Controllers
{
    public class AgentController : BaseAdminController
    {              
        private readonly IAgentQuery _agentQuery;
        private readonly IAgentCommand _agentCommand;
        private readonly ICompanyQuery _companyQuery;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AgentController(ILoggerService logger, IApplicationUserQuery applicationUserQuery,
            IAgentQuery agentQuery, IAgentCommand agentCommand, ICompanyQuery companyQuery,
             IWebHostEnvironment hostEnvironment) 
            : base(logger, applicationUserQuery)
        {            
            _agentQuery = agentQuery;
            _agentCommand = agentCommand;
            _companyQuery = companyQuery;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var agents = _agentQuery.GetAll(CurrentUser);
            return View(agents);
        }


        public IActionResult Upsert(Guid? id)
        {
            AgentViewModel agentViewModel = new()
            {
                Agent = new AgentDto(),
                CompanyList = _companyQuery.GetAll(CurrentUser).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };
            if (id == null)
            {
                //this is for create
                return View(agentViewModel);
            }
            //this is for edit
            agentViewModel.Agent = _agentQuery.GetById(id.GetValueOrDefault());
            if (agentViewModel.Agent == null)
            {
                return NotFound();
            }
            return View(agentViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(AgentViewModel agentViewModel)
        {
            var agentDto = agentViewModel.Agent;
            if (ModelState.IsValid)
            {
                _agentCommand.CurrentUser = CurrentUser.UserName;
                if (agentDto.Id == Guid.Empty)
                {
                    _agentCommand.Add(agentDto);
                }
                else
                {
                    _agentCommand.Update(agentDto);
                }
                _agentCommand.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                agentViewModel.CompanyList = _companyQuery.GetAll(CurrentUser).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if (agentDto.Id != Guid.Empty)
                {
                    agentDto = _agentQuery.GetById(agentDto.Id);
                }
            }
            return View(agentViewModel);
        }

        public IActionResult Import()
        {
            List<AgentDto> agents = new();
            return View(agents);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import(List<AgentDto> agents)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                string fileName = $"{files[0].FileName}_{Guid.NewGuid()}";
                var uploads = Path.Combine(webRootPath, @"data");
                var extenstion = Path.GetExtension(files[0].FileName);
                var path = Path.Combine(uploads, fileName + extenstion);
                using (var filesStreams = new FileStream(path, FileMode.Create))
                {
                    files[0].CopyTo(filesStreams);
                }

                agents = FileHelper.GetJsonData<AgentDto>(path);

                System.IO.File.Delete(path);
                                
                if (CurrentUser.Company == null)
                {
                    return Json(new { success = false, message = "Error while saving : Company not found" });
                }
                if (agents == null || agents.Count == 0)
                {
                    return Json(new { success = false, message = "Error while saving : Agents list is empty" });
                }

                List<AgentDto> allAgents = _agentQuery.GetAll(CurrentUser).ToList();
                _agentCommand.CurrentUser = CurrentUser.UserName;
                foreach (var agent in agents)
                {
                    agent.CompanyId = CurrentUser.CompanyId.GetValueOrDefault();
                    if (!allAgents.Exists(a => a.Number == agent.Number))
                        _agentCommand.Add(agent);
                }
                _agentCommand.Save();
            }
            return View(agents);
        }
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var agents = _agentQuery.GetAll(CurrentUser);
            return Json(new { data = agents });
        }

        //[HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var agent = _agentQuery.GetById(id);
            if (agent == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _agentCommand.DataBaseAction = DataBaseActionEnum.Delete;
            _agentCommand.Delete(id);
            _agentCommand.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion


    }

}