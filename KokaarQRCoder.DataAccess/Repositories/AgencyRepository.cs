using KokaarQrCoder.Domain.Entities;
using System;
using KokaarQrCoder.Domain.Contexts;
using KokaarQrCoder.DataAccess.Repositories.Contracts;

namespace KokaarQrCoder.DataAccess.Repositories
{
    public class AgencyRepository : BaseRepository<Agency, Guid>, IAgencyRepository
    {
        public AgencyRepository(ApplicationDbContext dbContext) : base(dbContext) 
        { 
        }

        public virtual void Update(Agency agencyToUpdate)
        {
            var originalEntity = GetById(agencyToUpdate.Id);

            if (!string.IsNullOrWhiteSpace(agencyToUpdate.Name)) originalEntity.Name = agencyToUpdate.Name;
            if (!string.IsNullOrWhiteSpace(agencyToUpdate.LocationUrl)) originalEntity.LocationUrl = agencyToUpdate.LocationUrl;
            originalEntity.CompanyId = agencyToUpdate.CompanyId;
            originalEntity.LastModificationDate = agencyToUpdate.LastModificationDate;
            originalEntity.LastModificationUser = agencyToUpdate.LastModificationUser;

            dbSet.Update(originalEntity);
        }
    }

}
