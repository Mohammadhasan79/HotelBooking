using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HotelService.Application.ApiClientInterface;
using HotelService.Application.Interfaces;
using HotelService.Application.ServiceInterface;
using HotelService.Infrastructure.ExternalService;
using HotelService.Infrastructure.Persistence;
using HotelService.Infrastructure.Repositories;
using HotelService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HotelService.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration )
        {
            services.AddDbContext<HotelDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString(
                        "DefaultConnection"));
            });
            services.AddScoped<IHotelRepository,HotelRepository>();

            services.AddScoped<IHotelService, HotelManagementService>();

            services.AddHttpClient<IBookingApiClient, BookingApiClient>((sp, client) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var baseUrl = configuration["ExternalServices:BookingServiceBaseUrl"];
                client.BaseAddress = new Uri(baseUrl!);
            });


            services.AddHttpClient<IRoomApiClient, RoomApiClient>((sp, client) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var baseUrl = configuration["ExternalServices:RoomServiceBaseUrl"];
                client.BaseAddress = new Uri(baseUrl!);
            });

            return services;
        }
    }
}
