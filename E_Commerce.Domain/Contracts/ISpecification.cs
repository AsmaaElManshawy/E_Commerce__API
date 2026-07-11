using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        //Includes related entities
        ICollection<Expression<Func<TEntity, object>>> IncludesExpressions { get; }
        Expression<Func<TEntity, bool>> Criteria { get; }
        Expression<Func<TEntity, object>> OrderBY { get; }
        Expression<Func<TEntity, object>> OrderBYDescending { get; }

        //Pagination
        int Skip { get; }
        int TAKE { get; }
        bool IsPaginated { get; }

    }
}
