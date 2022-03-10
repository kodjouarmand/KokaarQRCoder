using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.Domain.Entities;
using KokaarQrCoder.Utility.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KokaarQrCoder.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IApplicationUserQuery _applicationUserQuery;
        private readonly ICompanyQuery _companyQuery;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IApplicationUserQuery applicationUserQuery,
            ICompanyQuery companyQuery)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _applicationUserQuery = applicationUserQuery;
            _companyQuery = companyQuery;
        }

        public string UserName { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Name { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Company")]
            public Guid? CompanyId { get; set; }

            [Required]
            public string Role { get; set; }

            public IEnumerable<SelectListItem> CompanyList { get; set; }
            public IEnumerable<SelectListItem> RoleList { get; set; }

        }

        private void Load(ApplicationUser user)
        {
            UserName = user.UserName;
            var roleName = _applicationUserQuery.GetRoleName(user);
            Input = new InputModel
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                CompanyId = user.CompanyId,
                CompanyList = _companyQuery.GetAll(user).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Role = roleName,
                RoleList = _roleManager.Roles.Where(x => x.Name == roleName)
                    .Select(x => x.Name).Select(i => new SelectListItem
                    {
                        Text = i,
                        Value = i
                    })
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            //await LoadAsync(user);
            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                //await LoadAsync(user);
                Load(user);
                return Page();
            }

            user.Name = Input.Name;
            user.PhoneNumber = Input.PhoneNumber;
            user.CompanyId = Input.CompanyId;
            user.RoleName = Input.Role;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var lastRoleName = _applicationUserQuery.GetRoleName(user);
                await _userManager.RemoveFromRoleAsync(user, lastRoleName);
                await _userManager.AddToRoleAsync(user, user.RoleName);
            }
            else
            {
                throw new InvalidOperationException($"Unexpected error occurred when updating user with ID '{user.UserName}'.");
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
