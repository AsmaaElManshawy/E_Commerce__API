using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.BasketDtos
{
    public class BasketItemsDto
    {
        [Required(ErrorMessage = "Product Id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        [Range(1, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(1 , 50)]
        public int Quantity { get; set; }
    }
}
