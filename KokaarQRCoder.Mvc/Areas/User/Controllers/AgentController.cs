using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.Domain.Assemblers;
using KokaarQrCoder.Domain.ViewModels;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.Mvc;
using KokaarQrCoder.Utility.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace KokaarQrCoder.Mvc.Areas.User.Controllers
{
    public class AgentController : BaseUserController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAgentQuery _agentQuery;
        private readonly ICompanyQuery _companyQuery;
        public AgentController(ILoggerService logger, IApplicationUserQuery applicationUserQuery, IConfiguration configuration,
            IAgentQuery agentQuery, ICompanyQuery companyQuery, IWebHostEnvironment hostEnvironment)
            : base(logger, applicationUserQuery)
        {
            _agentQuery = agentQuery;
            _companyQuery = companyQuery;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var agents = _agentQuery.GetAll(CurrentUser);
            return Json(new { data = agents });
        }

        public IActionResult Generate(Guid? id)
        {
            AgentViewModel agentViewModel = new()
            {
                Agent = _agentQuery.GetById(id.GetValueOrDefault()),
                HasQRCode = false
            };

            return View(agentViewModel);

        }
        [HttpPost]
        public IActionResult Generate(AgentViewModel agentViewModel)
        {
            if (agentViewModel.Agent == null || agentViewModel.Agent.Id == Guid.Empty)
            {
                throw new Exception("Agent not found.");
            }

            var agentDto = _agentQuery.GetById(agentViewModel.Agent.Id);
            agentDto.Name = agentViewModel.Agent.Name;
            agentDto.PhoneNumber = agentViewModel.Agent.PhoneNumber;
            agentDto.Email = agentViewModel.Agent.Email;
            var companyDto = _companyQuery.GetById(agentDto.CompanyId);
            string webRootPath = _hostEnvironment.WebRootPath;
            var directoryName = Path.Combine(webRootPath, StaticHelper.QR_CODE_FOLDER_NAME);
            string fileName = $"{CommonHelper.RemoveSpecialCharacters(agentDto.Name)}.{StaticHelper.QR_CODE_IMAGE_EXTENSION}";
            string path = Path.Combine(directoryName, fileName);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            _agentQuery.GenerateQRCode(agentDto, companyDto, path);

            agentViewModel = new()
            {
                Agent = _agentQuery.GetById(agentDto.Id),
                HasQRCode = true
            };
            TempData["Success"] = $"QR code has been succesfully generated in the following directory {directoryName}.";
            return View(agentViewModel);
        }

        public IActionResult GenerateMany()
        {
            var agents = _agentQuery.GetAll(CurrentUser).ToList();
            if (agents == null || agents.Count == 0)
            {
                return Json(new { success = false, message = "Error while generating : Agents list is empty or null" });
            }

            try
            {
                foreach (var agent in agents)
                {
                    generateQRCode(agent, agent.Company);
                }
                return Json(new { success = true, message = $"QR codes for all agents have been succesfully generated." });
            }
            catch (Exception)
            {
               return Json(new { success = false, message = "Error while generating the QR code." });
            }
        }

        private void generateQRCode(AgentDto agent, CompanyDto company)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            var directoryName = Path.Combine(webRootPath, StaticHelper.QR_CODE_FOLDER_NAME);
            string fileName = $"{CommonHelper.RemoveSpecialCharacters(agent.Name)}.{StaticHelper.QR_CODE_IMAGE_EXTENSION}";
            string path = Path.Combine(directoryName, fileName);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            _agentQuery.GenerateQRCode(agent, company, path, true);
        }

        #endregion
    }
}