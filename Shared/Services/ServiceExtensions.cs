using Application;
using Application.Interfaces;
using Application.Services.StockS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Services
{
    public static class ServiceExtensions
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddScoped<StockService>();

        }
    }
}
