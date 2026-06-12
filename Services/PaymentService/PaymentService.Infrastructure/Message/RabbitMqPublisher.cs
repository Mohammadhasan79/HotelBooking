using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PaymentService.Application.Interfaces;
using RabbitMQ.Client;

namespace PaymentService.Infrastructure.Message
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        public async Task PublishAsync<T>(T message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "payment-completed",
                durable: true,
                exclusive: false,
                autoDelete: false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: "payment-completed",
                body: body);
        }
    }
}