using KokaarQrCoder.Domain.Entities;
using System;
using KokaarQrCoder.Domain.Contexts;
using KokaarQrCoder.DataAccess.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace KokaarQrCoder.DataAccess.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<ApplicationUser> dbSet;

        public ApplicationUserRepository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<ApplicationUser>();
        }

        public void Add(ApplicationUser entity)
        {
            dbSet.Add(entity);
        }

        public ApplicationUser Get(Guid id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<ApplicationUser> GetAll(Expression<Func<ApplicationUser, bool>> filter = null, Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null, string includeProperties = null)
        {
            IQueryable<ApplicationUser> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public ApplicationUser GetFirstOrDefault(Expression<Func<ApplicationUser, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<ApplicationUser> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }


            return query.FirstOrDefault();
        }

        public void Remove(Guid id)
        {
            ApplicationUser entity = dbSet.Find(id);
            Remove(entity);
        }

        public void Remove(ApplicationUser entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<ApplicationUser> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }

}
