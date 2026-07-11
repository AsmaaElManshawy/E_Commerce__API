using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace E_Commerce.Infrastructure.Repositories
{
    internal class GenericRepository<TEntity, TKey>(StoreDbContext dbContext)
    : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public async Task<TEntity> GetByIdAsync(TKey id, CancellationToken ct)
            => await dbContext.Set<TEntity>().FindAsync(id, ct);
        public async Task<TEntity> GetByIdAsync(ISpecification<TEntity, TKey> spec, CancellationToken ct)
        {
            var query = specificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), spec);
            return await query.FirstOrDefaultAsync(ct);
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct)
            => await dbContext.Set<TEntity>().ToListAsync(ct);
        public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> Spec, CancellationToken ct = default)
        {
            var query = specificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), Spec);

            return await query.ToListAsync();
        }
        public void Add(TEntity entity) => dbContext.Set<TEntity>().Add(entity);
        public void Update(TEntity entity) => dbContext.Set<TEntity>().Update(entity);
        public void Delete(TEntity entity) => dbContext.Set<TEntity>().Remove(entity);
        public async Task<int> CountAsync(ISpecification<TEntity, TKey> spec, CancellationToken ct = default)
            => await specificationEvaluator.CreateQuery(dbContext.Set<TEntity>(), spec).CountAsync(ct);

    }
}
