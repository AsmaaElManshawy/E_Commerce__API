using E_Commerce.Application.DTOs.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.OrdersDtos
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public string BuyerEmail { get; set; } = default!;
        public AddressDto ShippingAddress { get; set; } = default!;
        public string Status { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }
        public ICollection<OrderItemDto> Items { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }



    }
}
