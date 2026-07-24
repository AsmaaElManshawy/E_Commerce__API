using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public Order( string buyerEmail, OrderAddress shippingAddress, ICollection<OrderItem> items, 
            DeliveryMethod deliveryMethod, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            Items = items;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
        }

        public Order()
        {
            
        }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now; 
        public OrderStatus Status { get; set; } = OrderStatus.Pending; 
        public string BuyerEmail { get; set; } = default!;
        public OrderAddress ShippingAddress { get; set; } = default!;
        public ICollection<OrderItem> Items { get; set; } = [];
        public DeliveryMethod DeliveryMethod { get; set; }
        public decimal SubTotal { get; set; } 
        public int DeliveryMethodId { get; set; } // fk
        public decimal GetTotal()
            => SubTotal + (DeliveryMethod?.Price ?? 0);
    }
}
