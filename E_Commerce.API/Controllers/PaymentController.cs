using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.BasketDtos;
using E_Commerce.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class PaymentController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId, CancellationToken ct)
            => ToActionResult(await _paymentService.CreateOrUpdatePaymentIntentAsync(basketId, ct));


    }
}
