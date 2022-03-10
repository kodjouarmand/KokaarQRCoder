using KokaarQrCoder.Domain.Entities;
using KokaarQrCoder.DataAccess.Repositories.Contracts;
using KokaarQrCoder.BusinessLogic.Queries.Contracts;
using KokaarQrCoder.Utility.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System;

namespace KokaarQrCoder.BusinessLogic.Queries
{
    public class ApplicationUserQuery : IApplicationUserQuery
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly IUnitOfWork _unitOfWork;
        public ApplicationUserQuery(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public ApplicationUser GetUserById(Guid id)
        {
            var applicationUser = _unitOfWork.ApplicationUser.Get(id);
            return applicationUser;
        }

        public ApplicationUser GetCurrentUser(ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var applicationUser = _unitOfWork.ApplicationUser.Get(new Guid(claim.Value));
                return applicationUser;
            }
            return null;
        }

        public ApplicationUser GetCurrentUser(ISession session)
            => session.Get<ApplicationUser>(StaticHelper.SESSION_KEY_CURRENT_USER);

        public void SetCurrentUser(ISession session, ApplicationUser user)
            => session.Set<ApplicationUser>(StaticHelper.SESSION_KEY_CURRENT_USER, user);

        public bool IsSuperAdministrator(ApplicationUser user)
        {
            return _userManager.IsInRoleAsync(user, StaticHelper.ROLE_NAME_SUPER_ADMIN).Result;
        }

        public bool IsAdministrator(ApplicationUser user)
        {
            return _userManager.IsInRoleAsync(user, StaticHelper.ROLE_NAME_ADMIN).Result;
        }

        public string GetRoleName(ApplicationUser user)
        {
            var roleName = StaticHelper.ROLE_NAME_SIMPLE_USER;
            if (IsAdministrator(user))
                return StaticHelper.ROLE_NAME_ADMIN;
            if (IsSuperAdministrator(user))
                return StaticHelper.ROLE_NAME_SUPER_ADMIN;
            return roleName;
        }

        public void Update(ApplicationUser user)
        {
            _unitOfWork.ApplicationUser.Add(user);
            _unitOfWork.Save();
        }
    }
}
