using System;
using System.IO;
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
    public class CompanyController : BaseUserController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ICompanyQuery _companyQuery;
        private readonly IAgencyQuery _agencyQuery;
        private readonly ISocialNetworkAccountQuery _socialNetworkAccountQuery;

        public CompanyController(ILoggerService logger, IApplicationUserQuery applicationUserQuery, IConfiguration configuration,
            ICompanyQuery companyQuery, IAgencyQuery agencyQuery, ISocialNetworkAccountQuery socialNetworkAccountQuery,
            IWebHostEnvironment hostEnvironment)
            : base(logger, applicationUserQuery)
        {
            _companyQuery = companyQuery;
            _agencyQuery = agencyQuery;
            _socialNetworkAccountQuery = socialNetworkAccountQuery;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Generate(Guid? id)
        {
            CompanyViewModel companyViewModel = new()
            {
                Company = _companyQuery.GetById(id.GetValueOrDefault()),
                Agencies = _agencyQuery.GetByCompanyId(id.GetValueOrDefault()),
                SocialNetworkAccounts = _socialNetworkAccountQuery.GetByCompanyId(id.GetValueOrDefault()),
                HasQRCode = false
            };

            return View(companyViewModel);
        }

        [HttpPost]
        public IActionResult Generate(CompanyViewModel companyViewModel)
        {
            if(!ModelState.IsValid)
            {
                throw new Exception("Missing some required data. Please make sure that all the required fields have been filled.");
            }

            var companyDto = _companyQuery.GetById(companyViewModel.Company.Id);
            companyDto.PhoneNumber = companyViewModel.Company.PhoneNumber;
            companyDto.Email = companyViewModel.Company.Email;
            companyDto.WebSite = companyViewModel.Company.WebSite;

            string webRootPath = _hostEnvironment.WebRootPath;
            var directoryName = Path.Combine(webRootPath, StaticHelper.QR_CODE_FOLDER_NAME);
            string fileName = $"{CommonHelper.RemoveSpecialCharacters(companyDto.Name)}.{StaticHelper.QR_CODE_IMAGE_EXTENSION}";
            string path = Path.Combine(directoryName, fileName);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            _companyQuery.GenerateQRCode(companyDto, path);

            companyViewModel = new()
            {
                Company = companyDto,
                Agencies = _agencyQuery.GetByCompanyId(companyDto.Id),
                SocialNetworkAccounts = _socialNetworkAccountQuery.GetByCompanyId(companyDto.Id),
                HasQRCode = true
            };
            TempData["Success"] = $"QR code has been succesfully generated in the following directory {directoryName}.";
            return View(companyViewModel);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _companyQuery.GetAll(CurrentUser);
            return Json(new { data = companies });
        }
    }
}
