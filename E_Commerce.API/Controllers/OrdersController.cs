using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.OrdersDtos;
using E_Commerce.Domain.Entities.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace E_Commerce.API.Controllers
{
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        //   POST /api/Orders                  # Create order
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder([FromBody] OrderDto order, CancellationToken ct = default)
            => ToActionResult(await _orderService.CreateOrderAsync(order, GetEmailFromToken(), ct));

        //   GET  /api/Orders/{id}             # Get order by ID
        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id , CancellationToken ct= default)
             => ToActionResult(await _orderService.GetOrderByIdAndEmailUserAsync(id , GetEmailFromToken(), ct));
        //   GET  /api/Orders                  # Get user orders
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders( CancellationToken ct= default)
             => ToActionResult(await _orderService.GetAllOrdersForSpecificUserAsync(GetEmailFromToken(), ct));
        ///   GET  /api/Orders/deliveryMethods  # Get delivery methods
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethods(CancellationToken ct = default)
             => ToActionResult(await _orderService.GetAllDeliveryMethodsAsync(ct));
    }
}
