using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specification
{
    public class ProductCountSpecification : BaseSpecification<Product, int>
    {
        public ProductCountSpecification(ProuductQueryParams queryParams) : base
            (P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value) &&
                  (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value) &&
                  (string.IsNullOrEmpty(queryParams.Search) || P.Name.ToLower().Contains(queryParams.Search.ToLower())))
        {


        }
    
    }
}
