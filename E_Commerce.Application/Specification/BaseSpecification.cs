using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specification
{
    public class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludesExpressions { get; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public Expression<Func<TEntity, object>> OrderBY { get; private set; }

        public Expression<Func<TEntity, object>> OrderBYDescending { get; private set; }



        protected void AddInclude(Expression<Func<TEntity, object>> include)
        {
            IncludesExpressions.Add(include);
        }

        public BaseSpecification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBY = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression)
        {
            OrderBYDescending = orderByDescendingExpression;
        }
        // Pagination
        public int Skip { get; private set; }
        public int TAKE { get; private set; }
        public bool IsPaginated { get; private set; }

        public void ApplyPagination(int pagesize, int pageIndex)
        {
            Skip = (pageIndex - 1) * pagesize;
            TAKE = pagesize;
            IsPaginated = true;
        }
    }
}
