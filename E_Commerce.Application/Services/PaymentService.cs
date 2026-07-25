using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.BasketDtos;
using E_Commerce.Application.Specification;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentGateway _paymentGateway;
        private readonly IMapper _mapper;
        private readonly PaymentGatewaySettings _paymentSettings;
        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, 
            IPaymentGateway paymentGateway , IMapper mapper , IOptions<PaymentGatewaySettings> options)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentGateway = paymentGateway;
            _mapper = mapper;
            _paymentSettings = options.Value;
        }
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken ct = default)
        {
            // 1) Get Baket [Validate]
            var basket = await _basketRepository.GetBasketAsync(basketId , ct);
            if (basket == null)
                return Result<BasketDto>.Fail(Error.NotFound(description: "Basket Not Found"));
            if (basket.Items.Count == 0)
                return Result<BasketDto>.Fail(Error.NotFound(description: "Basket Is Empty"));

            // 2) DeliveryMethod
            if (!basket.DeliveryMethodId.HasValue)
                return Result<BasketDto>.Fail(Error.Validation());
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value, ct);
            if (deliveryMethod == null)
                return Result<BasketDto>.Fail(Error.NotFound(description: "Delivery Method Not Found"));

            // cost
            basket.ShippingPrice = deliveryMethod.Price;

            // 3) Product prices
            var productIds = basket.Items.Select(x => x.Id).ToHashSet();

            var products = (await _unitOfWork.GetRepository<Product, int>()
                .GetAllAsync( new ProductWithIdSpecification(productIds), ct)).ToDictionary(x => x.Id);

            foreach (var item in basket.Items)
            {
                if (!products.TryGetValue(item.Id, out var product))
                    return Result<BasketDto>.Fail(Error.NotFound(description: "Product Not Found"));

                item.Price = product.Price;
            }

            // 4) Total Amount
            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);

            var amount = (long)((subTotal + deliveryMethod.Price) * 100);

            // 5) PaymentIntentId => Empty => Create
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create
                var result = await _paymentGateway.CreatePaymentIntentAsync(amount, _paymentSettings.DefaultCurrancy, ct);

                basket.PaymentIntentId = result.PaymentIntentId;
                basket.ClientSecret = result.ClientSecret;
            }
            else // Update
                await _paymentGateway.UpdatePaymentIntentAsync( amount, basket.PaymentIntentId, ct);

            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return Result<BasketDto>.Ok(_mapper.Map<BasketDto>(basket));
        }
    }
}
