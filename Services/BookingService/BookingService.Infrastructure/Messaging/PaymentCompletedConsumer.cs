using System.Text;
using System.Text.Json;
using BookingService.Application.ServiceInterface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;

namespace BookingService.Infrastructure.Messaging
{
    public class PaymentCompletedConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IChannel? _channel;

        public PaymentCompletedConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Retry تا وصل بشه
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = _configuration["RabbitMQ:HostName"] ?? "localhost"
                    };
                    _connection = await factory.CreateConnectionAsync(stoppingToken);
                    _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);
                    await _channel.QueueDeclareAsync(
                        queue: "payment-completed",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        cancellationToken: stoppingToken);
                    var consumer = new AsyncEventingBasicConsumer(_channel);
                    consumer.ReceivedAsync += async (sender, eventArgs) =>
                    {
                        var body = eventArgs.Body.ToArray();
                        var json = Encoding.UTF8.GetString(body);
                        var paymentEvent = JsonSerializer.Deserialize<PaymentCompletedEvent>(json);
                        using var scope = _scopeFactory.CreateScope();
                        var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
                        await bookingService.ConfirmBookingAsync(paymentEvent!.BookingId);
                        await _channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                    };
                    await _channel.BasicConsumeAsync(
                        queue: "payment-completed",
                        autoAck: false,
                        consumer: consumer,
                        cancellationToken: stoppingToken);

                    
                    break;
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                   
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel is not null)
                await _channel.CloseAsync();
            if (_connection is not null)
                await _connection.CloseAsync();
            await base.StopAsync(cancellationToken);
        }
    }
}