using System;
using KokaarQrCoder.Domain.Entities;

namespace KokaarQrCoder.DataAccess.Repositories.Contracts
{
    public interface ISocialNetworkRepository : IBaseRepository<SocialNetwork, Guid>
    {
        public void Update(SocialNetwork socialNetwork);        
    }
}
