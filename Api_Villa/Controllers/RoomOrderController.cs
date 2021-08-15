using Business.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe.Checkout;

namespace Api_Villa.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoomOrderController : Controller
    {
        private readonly IRoomOrderRepository _repository;
        private readonly IEmailSender _emailSender;

        public RoomOrderController(IRoomOrderRepository repository, IEmailSender emailSender)
        {
            _repository = repository;
            _emailSender = emailSender;
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomOrderDetailsDto details)
        {
            if (ModelState.IsValid)
            {
                var result = await _repository.Create(details);
                return Ok(result);
            }
            return BadRequest(new ErrorModel
                {
                    ErrorMessage = "Error while creating Room Details/Booking"
                });
        }



        [HttpPost]
        public async Task<IActionResult> PaymentSuccessful([FromBody] RoomOrderDetailsDto details)
        {
            var service = new SessionService();
            var sessionDetails = service.Get(details.StripeSessionId);
            if (sessionDetails.PaymentStatus == "paid")
            {
                var result =await _repository.MarkPaymentSuccessful(details.Id);
                if (result == null)
                {
                    return BadRequest(new ErrorModel
                    {
                        ErrorMessage = "Cannot mark payment as successful"
                    });
                }

                await _emailSender.SendEmailAsync(details.Email, "Booking Confirmed - Villa",
                    "Your Booking has been confirmed at Villa with order Id: " + details.Id);
                return Ok(result);
            }

            return BadRequest(new ErrorModel
            {
                ErrorMessage = "Cannot mark payment as successful"
            });
        }


    }
}
