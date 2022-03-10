
namespace KokaarQrCoder.DataAccess.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        ICompanyRepository Company { get; }
        ISocialNetworkRepository SocialNetwork { get; }
        ISocialNetworkAccountRepository SocialNetworkAccount { get; }
        IAgencyRepository Agency { get; }
        IAgentRepository Agent { get; }
        IApplicationUserRepository ApplicationUser { get; }

        void Save();
    }

}
