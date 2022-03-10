using System;
using KokaarQrCoder.Domain.Entities;

namespace KokaarQrCoder.DataAccess.Repositories.Contracts
{
    public interface IAgentRepository : IBaseRepository<Agent, Guid>
    {
        public void Update(Agent agent);        
    }
}
