using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.OrdersDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IOrderService
    {
        // create order
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto , string email , CancellationToken ct = default );
        Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForSpecificUserAsync( string email , CancellationToken ct = default );
        Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailUserAsync(Guid id, string email , CancellationToken ct = default );
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default );
    }
}
