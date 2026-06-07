using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HotelService.Application.Interfaces;
using HotelService.Application.ServiceInterface;
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

            return services;
        }
    }
}
