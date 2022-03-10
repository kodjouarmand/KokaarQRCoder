using KokaarQrCoder.Domain.Entities;
using System;
using KokaarQrCoder.Domain.Contexts;
using KokaarQrCoder.DataAccess.Repositories.Contracts;

namespace KokaarQrCoder.DataAccess.Repositories
{
    public class CompanyRepository : BaseRepository<Company, Guid>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext dbContext) : base(dbContext) 
        { 
        }

        public virtual void Update(Company companyToUpdate)
        {
            var originalEntity = GetById(companyToUpdate.Id);

            if (!string.IsNullOrWhiteSpace(companyToUpdate.Name)) originalEntity.Name = companyToUpdate.Name;
            if (!string.IsNullOrWhiteSpace(companyToUpdate.Address)) originalEntity.Address = companyToUpdate.Address;
            if (!string.IsNullOrWhiteSpace(companyToUpdate.PhoneNumber)) originalEntity.PhoneNumber = companyToUpdate.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(companyToUpdate.Email)) originalEntity.Email = companyToUpdate.Email;
            if (!string.IsNullOrWhiteSpace(companyToUpdate.WebSite)) originalEntity.WebSite = companyToUpdate.WebSite;
            if (!string.IsNullOrWhiteSpace(companyToUpdate.AdditionnalInformations)) originalEntity.AdditionnalInformations = companyToUpdate.AdditionnalInformations;
            originalEntity.LastModificationDate = companyToUpdate.LastModificationDate;
            originalEntity.LastModificationUser = companyToUpdate.LastModificationUser;

            dbSet.Update(originalEntity);
        }
    }

}
