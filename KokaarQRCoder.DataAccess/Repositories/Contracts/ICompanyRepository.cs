using System;
using KokaarQrCoder.Domain.Entities;

namespace KokaarQrCoder.DataAccess.Repositories.Contracts
{
    public interface ICompanyRepository : IBaseRepository<Company, Guid>
    {
        public void Update(Company company);        
    }
}
