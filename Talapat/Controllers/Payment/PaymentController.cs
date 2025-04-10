using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using TalabatCore.Entities.Basket;
using TalabatCore.Repositories;
using TalabatCore.Services;
using TalabatCore.Services.IPayment;
using Talapat.DTOs.Basket;
using Talapat.Errors;

namespace Talapat.Controllers.Payment
{
    public class PaymentController : BaseAPIController
    {
        private readonly string endpointSecret = "your_webhook_secret_here";
        public PaymentController(IBasketRepositories BasketRepe, IPaymentService PaymentService, IMapper mapper)
        {
            this._BasketRepe = BasketRepe;
            this._PaymentService = PaymentService;
            _Mapper = mapper;
        }

        public IBasketRepositories _BasketRepe { get; }
        public IPaymentService _PaymentService { get; }
        public IMapper _Mapper { get; }

        // CreateOrUpdate
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string BasketId)
        {
            
            var Basket = await _PaymentService.CreateOrUpdatePaymentIntent(BasketId);
            if(Basket is null)
            {
                return BadRequest(new ApiResponse(400, "Problem with Payment"));
            }
            var mappedBasket = _Mapper.Map<CustomerBasket, CustomerBasketDto>(Basket);
            return Ok(mappedBasket);
        }
        // Inside the webhook method
        [HttpPost("webhook")]
        public async Task<IActionResult> wehbhook()
        {
            try
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    endpointSecret
                );
                var PaymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    await _PaymentService.UpdatePaymentIntentToFailedOrSucced(PaymentIntent.Id, false);

                }
                else if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    await _PaymentService.UpdatePaymentIntentToFailedOrSucced(PaymentIntent.Id, true);
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch(StripeException e)
            {
                return BadRequest(new ApiResponse(400, e.Message));
            }
           
        }


    }
}
