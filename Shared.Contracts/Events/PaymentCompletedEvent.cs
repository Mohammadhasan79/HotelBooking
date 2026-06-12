using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Events
{
    public class PaymentCompletedEvent
    {
        public int BookingId { get; set; }

        public int PaymentId { get; set; }

        public decimal Amount { get; set; }

        public DateTime CompletedAt { get; set; }
    }
}
