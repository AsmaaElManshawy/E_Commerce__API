using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.OrdersDtos;
using E_Commerce.Application.Specification;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IBasketRepository basketRepository , IUnitOfWork unitOfWork , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default)
        {
            // Items[order items] => Basket
            var basket = await _basketRepository.GetBasketAsync(orderDto.BasketId, ct);
            if (basket == null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Not Found" , $"Basket With This Id = {orderDto.BasketId} Not Found"));

            if (basket.Items.Count == 0)
                return Result<OrderToReturnDto>.Fail(Error.Validation("Validation", "Basket is Empty"));

            var orderItems = new List<OrderItem>(basket.Items.Count);
            var productIds = basket.Items.Select(p => p.Id).ToHashSet();
            var products = (await _unitOfWork.GetRepository<Product, int>()
                    .GetAllAsync(new ProductWithIdSpecification(productIds), ct)).ToDictionary(x => x.Id);
            
            foreach (var item in basket.Items)
            {
                
                if (!products.TryGetValue( item.Id , out var product ))
                    return Result<OrderToReturnDto>.Fail(Error.NotFound("Not Found", $"Product With This Id = {item.Id} Not Found"));

                // add to  orderItems list
                orderItems.Add(new OrderItem()
                {
                    Price = product.Price, // from Database
                    Quantity = item.Quantity,
                    Product = new ProductItemOrdered()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl,
                    }
                });
            }

            // ship to address
            var orderAddress = _mapper.Map<OrderAddress>(orderDto.ShipToAddress);
            // delivery method
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId, ct);
            if (deliveryMethod == null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Not Found", $"Delivery Method With This Id = {orderDto.DeliveryMethodId} Not Found"));
            // sub total
            // product price * Quantity
            var subTotal = orderItems.Sum(x => x.Quantity * x.Price);
            // create order
            var order = new Order(email , orderAddress , orderItems , deliveryMethod , subTotal);

            _unitOfWork.GetRepository<Order , Guid>().Add(order);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            if (result > 0)
            {
                await _basketRepository.DeleteBasketAsync(orderDto.BasketId, ct);
                return Result<OrderToReturnDto>.Ok(_mapper.Map<OrderToReturnDto>(order));
            }
            else
                return Result<OrderToReturnDto>.Fail(Error.Failure(description: "Failed to Add Order"));
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync(CancellationToken ct = default)
        {
            var deliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetAllAsync(ct);
            if (deliveryMethods.Any())
                return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(_mapper.Map<IReadOnlyList<DeliveryMethodDto>>(deliveryMethods));
            else
                return Result<IReadOnlyList<DeliveryMethodDto>>.Fail(Error.NotFound(description: "No Delivery Method Found"));
        }

        public async Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersForSpecificUserAsync(string email, CancellationToken ct = default)
        {
            var orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(new OrderSpecification(email), ct);
            if (orders.Any())
                return Result<IReadOnlyList<OrderToReturnDto>>.Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
            else
                return Result<IReadOnlyList<OrderToReturnDto>>.Fail(Error.NotFound(description: "No Orders Found"));
        }

        public async Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailUserAsync( Guid id,string email, CancellationToken ct = default)
        {
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderSpecification(id ,email), ct);
            if (order != null)
                return Result<OrderToReturnDto>.Ok(_mapper.Map<OrderToReturnDto>(order));
            else
                return Result<OrderToReturnDto>.Fail(Error.NotFound(description: "Order Not Found"));
        }
    }
}
