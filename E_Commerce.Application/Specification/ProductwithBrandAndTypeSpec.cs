using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specification
{
    public class ProductwithBrandAndTypeSpec : BaseSpecification<Product, int>
    {
        public ProductwithBrandAndTypeSpec(ProuductQueryParams queryParams) : base
            (P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value) &&
                  (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value) &&
                  (string.IsNullOrEmpty(queryParams.Search) || P.Name.ToLower().Contains(queryParams.Search.ToLower())))
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

            switch (queryParams.Sort)
            {
                case ProudectSortOptions.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case ProudectSortOptions.PriceDesc:
                    AddOrderByDescending(P => P.Price);
                    break;
                case ProudectSortOptions.NameDesc:
                    AddOrderByDescending(P => P.Name);
                    break;
                default:
                    AddOrderBy(P => P.Id);
                    break;
            }

            // Pagination: تأكد أن المعاملات تتوافق مع تعريف ApplyPagination في BaseSpecification
            ApplyPagination(queryParams.pageSize, queryParams.PageIndex);
        }

        public ProductwithBrandAndTypeSpec(int id) : base(P => P.Id == id)
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
    }
}
