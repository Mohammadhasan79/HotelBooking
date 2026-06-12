using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Interfaces;
using PaymentService.Infrastructure.Message;
using PaymentService.Infrastructure.Persistence;
using PaymentService.Infrastructure.Repository;
using PaymentService.Infrastructure.Services;

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
            services.AddScoped<IPaymentService, PaymentServiceManagement>();
            services.AddScoped<IMessagePublisher,RabbitMqPublisher>();
            return services;
        }
    }
}