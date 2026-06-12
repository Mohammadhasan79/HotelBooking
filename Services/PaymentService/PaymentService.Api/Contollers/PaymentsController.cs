using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Interfaces;
using Shared.Contracts.Events;

namespace PaymentService.Api.Contollers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;


        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            await _paymentService.CompletePaymentAsync(id);
            return Ok();
        }
    }
}
