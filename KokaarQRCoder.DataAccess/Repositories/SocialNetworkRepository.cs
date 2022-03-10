using KokaarQrCoder.Domain.Entities;
using System;
using KokaarQrCoder.Domain.Contexts;
using KokaarQrCoder.DataAccess.Repositories.Contracts;

namespace KokaarQrCoder.DataAccess.Repositories
{
    public class SocialNetworkRepository : BaseRepository<SocialNetwork, Guid>, ISocialNetworkRepository
    {
        public SocialNetworkRepository(ApplicationDbContext dbContext) : base(dbContext) 
        { 
        }

        public virtual void Update(SocialNetwork socialNetworkToUpdate)
        {
            var originalEntity = GetById(socialNetworkToUpdate.Id);

            if (!string.IsNullOrWhiteSpace(socialNetworkToUpdate.Name)) originalEntity.Name = socialNetworkToUpdate.Name;
            originalEntity.LastModificationDate = socialNetworkToUpdate.LastModificationDate;
            originalEntity.LastModificationUser = socialNetworkToUpdate.LastModificationUser;

            dbSet.Update(originalEntity);
        }
    }

}
