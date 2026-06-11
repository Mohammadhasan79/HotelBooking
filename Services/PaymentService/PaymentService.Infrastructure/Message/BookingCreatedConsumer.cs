using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using PaymentService.Domain.Entities;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Enums;
using Shared.Contracts.Events;
using Microsoft.Extensions.Hosting;

namespace PaymentService.Infrastructure.Message
{
    public class BookingCreatedConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection? _connection;
        private IChannel? _channel;

        public BookingCreatedConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(
                queue: "booking-created",
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, eventArgs) =>
            {
                var body = eventArgs.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var bookingEvent = JsonSerializer.Deserialize<BookingCreatedEvent>(json);

                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider
                    .GetRequiredService<IPaymentRepository>();

                try
                {
                    var payment = new Payment
                    {
                        BookingId = bookingEvent!.BookingId,
                        UserId = bookingEvent.UserId,
                        Amount = bookingEvent.TotalPrice,
                        Status = PaymentStatus.Pending,
                        CreatedAt = DateTime.UtcNow
                    };

                    await repository.AddAsync(payment);
                    await repository.SaveChangesAsync();

                    await _channel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
                }
                catch
                {
                    await _channel.BasicNackAsync(eventArgs.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: "booking-created",
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel is not null)
                await _channel.CloseAsync(cancellationToken);

            if (_connection is not null)
                await _connection.CloseAsync(cancellationToken);

            await base.StopAsync(cancellationToken);
        }
    }
}