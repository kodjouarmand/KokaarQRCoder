using System;
using KokaarQrCoder.Domain.Entities;

namespace KokaarQrCoder.DataAccess.Repositories.Contracts
{
    public interface ISocialNetworkAccountRepository : IBaseRepository<SocialNetworkAccount, Guid>
    {
        public void Update(SocialNetworkAccount socialNetworkAccount);        
    }
}
