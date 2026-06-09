using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoomService.Application.Interfaces;
using RoomService.Application.RepositoryInterface;
using RoomService.Application.ServiceInterface;
using RoomService.Infrastructure.ExternalServices;
using RoomService.Infrastructure.Persistence;
using RoomService.Infrastructure.Repository;
using RoomService.Infrastructure.Service;

namespace RoomService.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<RoomDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<IRoomService, RoomServiceManagement>();
            services.AddHttpClient<IHotelApiClient, HotelApiClient>(client =>
            {
                client.BaseAddress =
                    new Uri("https://localhost:7273/");
            });
            return services;
        }
    }
}
