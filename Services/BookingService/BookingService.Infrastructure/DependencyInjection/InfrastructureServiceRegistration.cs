using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingService.Application.RepositoryInterface;
using BookingService.Application.ServiceInterface;
using BookingService.Infrastructure.ExternalServices;
using BookingService.Infrastructure.Persistence;
using BookingService.Infrastructure.Repository;
using BookingService.Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingService.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration )
        {
            services.AddDbContext<BookingDbContext>(option =>
            {
                option.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingService, BookingServiceManagement>();
            services.AddHttpClient<IRoomApiClient, RoomApiClient>((sp, client) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var baseUrl = configuration["ExternalServices:RoomServiceBaseUrl"];
                client.BaseAddress = new Uri(baseUrl!);
            });
            services.AddScoped<IMessagePublisher, RabbitMqPublisher>();

            return services;
        }
    }
}
