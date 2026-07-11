using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id , CancellationToken ct = default);
        Task<TEntity?> GetByIdAsync(ISpecification<TEntity, TKey> spec , CancellationToken ct = default);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> spec, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<int> CountAsync(ISpecification<TEntity, TKey> spec, CancellationToken ct = default);

    }
}
