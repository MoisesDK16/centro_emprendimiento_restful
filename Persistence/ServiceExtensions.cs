﻿using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Persistence.Repository;

namespace Persistence
{
    public static class ServiceExtensions
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("cn"),
                    b => b.MigrationsAssembly("WebAPI")),
                    ServiceLifetime.Scoped
                );

            services.AddTransient(typeof(IRepositoryAsync<>), typeof(MyRepositoryAsync<>));
            services.AddTransient(typeof(IReadOnlyRepositoryAsync<>), typeof(MyRepositoryAsync<>));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("Caching:RedisConnection");
            });

            // Registrar el servicio de validación del esquema
            //services.AddScoped<DatabaseSchemaValidator>();

        }
    }
}
    