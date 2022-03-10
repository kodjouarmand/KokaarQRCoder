using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KokaarQrCoder.Domain.Contexts;
using KokaarQrCoder.Domain.Entities;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.Utility.Helpers;
using KokaarQrCoder.Utility.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace KokaarQrCoder.API.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ILoggerService _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IOptions<SuperAministratorOptions> _superAministratorOptions;

        public DbInitializer(ILoggerService logger, ApplicationDbContext dbContext, IWebHostEnvironment hostEnvironment,
             IOptions<SuperAministratorOptions> superAministratorOptions)
        {
            _logger = logger;
            _dbContext = dbContext;
            _hostEnvironment = hostEnvironment;
            _superAministratorOptions = superAministratorOptions;
        }

        public void Initialize()
        {
            try
            {
                Migrate();

                _logger.LogInformation($"Database initialization start ...");
                InitializeSuperAdministrator(_superAministratorOptions);
                InitializeApplicationUser();
                InitializeApplicationRole();
                InitializeApplicationUserRole();
                InitializeCompany();
                if (_hostEnvironment.IsDevelopment())
                {
                    InitializeAgency();
                    InitializeAgent();
                    InitializeSocialNetwork();
                    InitializeSocialNetworkAccount();
                }
                SaveChanges();
                _logger.LogInformation($"Database initialization end with success ...");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Database initialization failed...");
                _logger.LogError(ex);
                _logger.LogDebug($"Database initialization failed : {ex.Message}");
                throw;
            }
        }

        private void Migrate()
        {
            if (_hostEnvironment.IsDevelopment())
            {
                _dbContext.Database.EnsureDeleted();
            }
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                _logger.LogInformation($"Database migration start ...");
                _dbContext.Database.Migrate();
                _logger.LogInformation($"Database migration end with success ...");
            }
        }

        private void InitializeCompany()
        {
            if (!_dbContext.Companies.Any())
            {
                var companies = GetInitData<Company>(nameof(Company));
                _dbContext.AddRange(companies);
            }
        }

        private void InitializeAgency()
        {
            if (!_dbContext.Agencies.Any())
            {
                var agencies = GetInitData<Agency>(nameof(Agency));
                _dbContext.AddRange(agencies);
            }
        }

        private void InitializeAgent()
        {
            if (!_dbContext.Agents.Any())
            {
                var agents = GetInitData<Agent>(nameof(Agent));
                _dbContext.AddRange(agents);
            }
        }

        private void InitializeSocialNetwork()
        {
            if (!_dbContext.SocialNetworks.Any())
            {
                var seedSocialNetwork = GetInitData<SocialNetwork>(nameof(SocialNetwork));
                _dbContext.AddRange(seedSocialNetwork);
            }
        }

        private void InitializeSocialNetworkAccount()
        {
            if (!_dbContext.SocialNetworkAccounts.Any())
            {
                var socialNetworkAccounts = GetInitData<SocialNetworkAccount>(nameof(SocialNetworkAccount));
                _dbContext.AddRange(socialNetworkAccounts);
            }
        }

        private void InitializeApplicationRole()
        {
            if (!_dbContext.ApplicationRoles.Any())
            {
                var roles = GetInitData<ApplicationRole>(nameof(ApplicationRole));
                _dbContext.AddRange(roles);
            }
        }

        private void InitializeApplicationUser()
        {
            if (!_dbContext.ApplicationUsers.Any())
            {
                var users = GetInitData<ApplicationUser>(nameof(ApplicationUser));
                foreach (var user in users)
                {
                    user.NormalizedUserName = user.UserName.ToUpper();
                    user.NormalizedEmail = user.Email.ToUpper();
                    user.SecurityStamp = Guid.NewGuid().ToString("D");
                    user.PasswordHash = GeneratePassword(user, "Password123!");
                }
                _dbContext.AddRange(users);
            }
        }

        private void InitializeApplicationUserRole()
        {
            if (!_dbContext.ApplicationUserRoles.Any())
            {
                var userRoles = GetInitData<ApplicationUserRole>(nameof(ApplicationUserRole));
                _dbContext.AddRange(userRoles);
            }
        }

        private void InitializeSuperAdministrator(IOptions<SuperAministratorOptions> superAministratorOptions)
        {
            var superAministratorSettings = superAministratorOptions.Value;
            if (!_dbContext.ApplicationUsers.Any(u => u.Email.Equals(superAministratorSettings.Email)))
            {
                ApplicationUser superAdmin = new()
                {
                    Id = new Guid(superAministratorSettings.UserId),
                    UserName = superAministratorSettings.UserName,
                    NormalizedUserName = superAministratorSettings.UserName.ToUpper(),
                    Name = superAministratorSettings.Name,
                    Email = superAministratorSettings.Email,
                    NormalizedEmail = superAministratorSettings.Email.ToUpper(),
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                superAdmin.PasswordHash = GeneratePassword(superAdmin, superAministratorSettings.Password);

                _dbContext.Add(superAdmin);
            }

            if (!_dbContext.ApplicationRoles.Any(u => u.Name.Equals(superAministratorSettings.RoleName)))
            {
                ApplicationRole superAdminRole = new()
                {
                    Id = new Guid(superAministratorSettings.RoleId),
                    Name = superAministratorSettings.RoleName,
                    NormalizedName = superAministratorSettings.RoleName.ToUpper()
                };
                _dbContext.Add(superAdminRole);
            }

            if (!_dbContext.ApplicationUserRoles.Any(u => u.UserId.ToString() == superAministratorSettings.UserId
                                                        && u.RoleId.ToString() == superAministratorSettings.RoleId))
            {
                ApplicationUserRole superAdminUserRole = new()
                {
                    RoleId = new Guid(superAministratorSettings.RoleId),
                    UserId = new Guid(superAministratorSettings.UserId)
                };
                _dbContext.Add(superAdminUserRole);
            }
        }

        private void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        private static string GeneratePassword(ApplicationUser user, string password)
        {
            var passHash = new PasswordHasher<ApplicationUser>();
            return passHash.HashPassword(user, password);
        }

        private List<T> GetInitData<T>(string fileName)
        {
            string fullFileName = GetInitDataDirectory(fileName);
            return FileHelper.GetJsonData<T>(fullFileName);
        }

        private string GetInitDataDirectory(string fileName)
        {
            if (_hostEnvironment.IsDevelopment())
            {
                fileName = $"{fileName}.Development.json";
            }
            else
            {
                fileName = $"{fileName}.Production.json";
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            string directoryName = Path.Combine(webRootPath, StaticHelper.INIT_DATA_PATH);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            return Path.Combine(directoryName, fileName);
        }
    }
}
