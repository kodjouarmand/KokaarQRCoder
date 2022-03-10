using KokaarQrCoder.Domain.Entities;
using System;
using KokaarQrCoder.Domain.Contexts;
using KokaarQrCoder.DataAccess.Repositories.Contracts;

namespace KokaarQrCoder.DataAccess.Repositories
{
    public class SocialNetworkAccountRepository : BaseRepository<SocialNetworkAccount, Guid>, ISocialNetworkAccountRepository
    {
        public SocialNetworkAccountRepository(ApplicationDbContext dbContext) : base(dbContext) 
        { 
        }

        public virtual void Update(SocialNetworkAccount socialNetworkAccountToUpdate)
        {
            var originalEntity = GetById(socialNetworkAccountToUpdate.Id);

            if (!string.IsNullOrWhiteSpace(socialNetworkAccountToUpdate.Account)) originalEntity.Account = socialNetworkAccountToUpdate.Account;
            originalEntity.SocialNetworkId = socialNetworkAccountToUpdate.SocialNetworkId;
            originalEntity.CompanyId = socialNetworkAccountToUpdate.CompanyId;
            originalEntity.LastModificationDate = socialNetworkAccountToUpdate.LastModificationDate;
            originalEntity.LastModificationUser = socialNetworkAccountToUpdate.LastModificationUser;

            dbSet.Update(originalEntity);
        }
    }

}
