using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingService.Application.ServiceInterface
{
    public interface IMessagePublisher
    {
        Task Publish<T>(T message);
    }
}
