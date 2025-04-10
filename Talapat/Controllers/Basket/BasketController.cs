using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using TalabatCore.Entities.Basket;
using TalabatCore.Repositories;
using Talapat.Errors;

namespace Talapat.Controllers.Basket
{
    public class BasketController : BaseAPIController
    {
        public IBasketRepositories _BasketRepo { get; }
        public BasketController(IBasketRepositories BasketRepo)
        {
            _BasketRepo = BasketRepo;
        }
        //Get Customer Basket
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId)
        {
            var CustomerBasket = await _BasketRepo.GetBasketAsync(BasketId); 
            if(CustomerBasket is null)
                return null;
            return Ok(CustomerBasket);
        }
        // Update or Create Customer Basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateCustomerBasket(CustomerBasket basket)
        {
            var UpdatedOrCreated = await _BasketRepo.UpdateBasketAsync(basket);
            if (UpdatedOrCreated is null)
                return BadRequest(new ApiResponse(400, "Cant Create Or Update"));
            return UpdatedOrCreated;
        }
        // Delete Customer Basket
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCustomerBaskte(string BasketId)
        {
            return await _BasketRepo.DeleteBasketAsync(BasketId);
        }
    }
}
