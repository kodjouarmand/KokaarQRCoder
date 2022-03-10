using System;
using KokaarQrCoder.Domain.Entities;

namespace KokaarQrCoder.DataAccess.Repositories.Contracts
{
    public interface IAgencyRepository : IBaseRepository<Agency, Guid>
    {
        public void Update(Agency agency);        
    }
}
