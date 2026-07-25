using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.BasketDtos
{
    public class BasketDto
    {
        public string Id { get; set; }
        public ICollection<BasketItemsDto> Items { get; set; }

        // paymennt
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
    }
}
