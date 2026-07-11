using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.BasketDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        // GET: BaseUrl/api/Basket/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> GetBasket(string id, CancellationToken ct)
        {
            var basket = await _basketService.GetBasketAsync(id, ct);
            return ToActionResult(basket);
        }
        // Post: BaseUrl/api/Basket => {body}
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket([FromBody] BasketDto basket, CancellationToken ct)
        {
            var result = await _basketService.CreateOrUpdateBasketAsync(basket, ct: ct);
            return ToActionResult(result);
        }

        // Delete: BaseUrl/api/Basket/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct)
        {
            var result = await _basketService.DeleteBasketAsync(id, ct);
            return ToActionResult(result);
        }
    }
}
