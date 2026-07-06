using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.ProductsDtos
{
    public class ProductDto
    {
        // Id, Name, Description, PictureUrl, Price, ProductBrand, ProductType
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public string ProductBrand { get; set; }
        public string ProductType { get; set; }
        public decimal Price { get; set; }
    }
}
