using KokaarQrCoder.Domain.Entities;
using System;
using KokaarQrCoder.Domain.Contexts;
using KokaarQrCoder.DataAccess.Repositories.Contracts;

namespace KokaarQrCoder.DataAccess.Repositories
{
    public class AgentRepository : BaseRepository<Agent, Guid>, IAgentRepository
    {
        public AgentRepository(ApplicationDbContext dbContext) : base(dbContext) 
        { 
        }

        public virtual void Update(Agent agentToUpdate)
        {
            var originalEntity = GetById(agentToUpdate.Id);

            if (!string.IsNullOrWhiteSpace(agentToUpdate.Name)) originalEntity.Name = agentToUpdate.Name;
            if (!string.IsNullOrWhiteSpace(agentToUpdate.PhoneNumber)) originalEntity.PhoneNumber = agentToUpdate.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(agentToUpdate.Email)) originalEntity.Email = agentToUpdate.Email;
            originalEntity.CompanyId = agentToUpdate.CompanyId;
            originalEntity.LastModificationDate = agentToUpdate.LastModificationDate;
            originalEntity.LastModificationUser = agentToUpdate.LastModificationUser;

            dbSet.Update(originalEntity);
        }
    }

}
