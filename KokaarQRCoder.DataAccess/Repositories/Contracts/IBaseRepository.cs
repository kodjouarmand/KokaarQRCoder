using KokaarQrCoder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KokaarQrCoder.DataAccess.Repositories.Contracts
{
    public interface IBaseRepository<TEntity, TEntityKey> where TEntity : BaseEntity<TEntityKey>
    {
        TEntity GetById(TEntityKey id);

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> fliter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null);

        IQueryable<TEntity> Get( Expression<Func<TEntity, bool>> fliter = null,
           string includeProperties = null);

        void Add(TEntity entity);

        void Delete(TEntityKey id);
    }
}
