using System;
using MemberManagement.AppCore.Entities;
using MemberManagement.AppCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagement.Data
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MemberManagementContext _dbContext;

        public EfRepository(MemberManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);

            // Return null if the entity was found, but it is deleted
            return entity != null && !entity.Archived ? entity : null;
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>()
                .Where(e => !e.Archived)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(_dbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            return await secondaryResult
                .Where(e => !e.Archived)
                .AsNoTracking()
                .ToListAsync();
        }

        public IEnumerable<T> List()
        {
            return _dbContext.Set<T>()
                .AsNoTracking()
                .AsEnumerable();
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.Created = DateTime.Now;
            entity.LastUpdated = DateTime.Now;
            entity.Archived = false;

            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            entity.LastUpdated = DateTime.Now;

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            entity.Archived = true;

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
