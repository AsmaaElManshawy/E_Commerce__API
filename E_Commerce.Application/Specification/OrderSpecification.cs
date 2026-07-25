using E_Commerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specification
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(string email) : base(x => x.BuyerEmail == email)
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.Items);
            AddOrderByDescending(x => x.OrderDate);
        }
        public OrderSpecification(Guid id,string email) : base(x => x.BuyerEmail == email && x.Id == id)
        {
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.Items);
            AddOrderByDescending(x => x.OrderDate);
        }
    }
}
