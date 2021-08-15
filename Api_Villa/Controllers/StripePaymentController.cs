using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Models;
using Stripe.Checkout;
using Session = Stripe.BillingPortal.Session;

namespace Api_Villa.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StripePaymentController : Controller
    {
        private readonly IConfiguration _configuration;

        public StripePaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Create(StripePaymentDto payment)
        {
            try
            {
                var domain = _configuration.GetValue<string>("Villa_Client_Url");
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = payment.Amount,
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = payment.ProductName,
                                }
                            },
                            Quantity = 1
                        }
                    },
                    Mode = "payment",
                    SuccessUrl = domain + "/success-payment?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = domain + payment.ReturnUrl
                };

                var service = new SessionService();
                Stripe.Checkout.Session session = await service.CreateAsync(options);

                return Ok(new SuccessModel
                {
                    Data = session.Id
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorModel()
                {
                    ErrorMessage = e.Message
                });
            }
        }



    }
}
