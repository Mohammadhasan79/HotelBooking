using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Enums;
using Shared.Contracts.Events;

namespace PaymentService.Infrastructure.Services
{
    public class PaymentServiceManagement : IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IMessagePublisher _publisher;

        public PaymentServiceManagement(IPaymentRepository repository, IMessagePublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        public async Task CompletePaymentAsync(int paymentId)
        {
            var payment = await _repository.GetByIdAsync(paymentId);
            if (payment == null) throw new Exception("Payment not found");

            payment.Status = PaymentStatus.Completed;
            await _repository.SaveChangesAsync();

            var paymentCompletedEvent = new PaymentCompletedEvent
            {
                PaymentId = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                CompletedAt = DateTime.UtcNow
            };

            await _publisher.PublishAsync(paymentCompletedEvent);
        }
    }
}