using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Interfaces;
using PaymentService.Infrastructure.Persistence;
using PaymentService.Infrastructure.Repository;

namespace PaymentService.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection
            AddInfrastructure(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            services.AddDbContext<
                PaymentDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                });
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            return services;
        }
    }
}