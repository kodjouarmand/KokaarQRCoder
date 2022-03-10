using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using KokaarQrCoder.Domain.Entities;

namespace KokaarQrCoder.DataAccess.Repositories.Contracts
{
    public interface IApplicationUserRepository
    {
        ApplicationUser Get(Guid id);

        IEnumerable<ApplicationUser> GetAll(
            Expression<Func<ApplicationUser, bool>> filter = null,
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null,
            string includeProperties = null
            );

        ApplicationUser GetFirstOrDefault(
            Expression<Func<ApplicationUser, bool>> filter = null,
            string includeProperties = null
            );

        void Add(ApplicationUser entity);
        void Remove(Guid id);
        void Remove(ApplicationUser entity);
        void RemoveRange(IEnumerable<ApplicationUser> entity);


    }
}
