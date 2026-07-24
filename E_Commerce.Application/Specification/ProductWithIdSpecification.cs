using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specification
{
    public class ProductWithIdSpecification : BaseSpecification<Product, int>
    {
        public ProductWithIdSpecification(HashSet<int> ProductIds) : base( p => ProductIds.Contains(p.Id))
        {
        }
    }
}
