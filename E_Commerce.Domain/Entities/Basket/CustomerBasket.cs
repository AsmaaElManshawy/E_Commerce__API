using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Basket
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public ICollection<BasketItems> Items { get; set; } = [];

        // paymennt
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
    }
}
