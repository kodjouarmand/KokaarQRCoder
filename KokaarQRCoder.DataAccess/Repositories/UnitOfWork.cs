using KokaarQrCoder.Domain.Contexts;
using KokaarQrCoder.DataAccess.Repositories.Contracts;
using System;

namespace KokaarQrCoder.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public ICompanyRepository Company { get; private set; }
        public ISocialNetworkRepository SocialNetwork { get; private set; }
        public ISocialNetworkAccountRepository SocialNetworkAccount { get; private set; }
        public IAgencyRepository Agency { get; private set; }
        public IAgentRepository Agent { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Company = new CompanyRepository(_dbContext);
            SocialNetwork = new SocialNetworkRepository(_dbContext);
            SocialNetworkAccount = new SocialNetworkAccountRepository(_dbContext);
            Agency = new AgencyRepository(_dbContext);
            Agent = new AgentRepository(_dbContext);
            ApplicationUser = new ApplicationUserRepository(_dbContext);
        }
        

        public void Dispose() => _dbContext.Dispose();

        public void Save() => _dbContext.SaveChanges();
    }

}
