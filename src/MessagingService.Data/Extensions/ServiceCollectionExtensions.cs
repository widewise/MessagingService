using System;
using System.Diagnostics.CodeAnalysis;
using MessagingService.Data.Repositories;
using MessagingService.Extensions;
using MessagingService.Repositories.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace MessagingService.Data.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddData(
            this IServiceCollection services,
            IConfiguration configuration,
            string redisConnectionStringName)
        {
            Preconditions.CheckNonNullOrEmpty(redisConnectionStringName, nameof(redisConnectionStringName));

            var redisConnectionString = configuration.GetConnectionString(redisConnectionStringName);
            Preconditions.CheckNonNullOrEmpty(redisConnectionStringName, nameof(redisConnectionString));

            services.AddSingleton(s => ConnectionMultiplexer.Connect(redisConnectionString));
            services.AddSingleton(GetRedisDatabase);
            
            services.AddTransient<IMessageRepository, MessageRepository>();

            return services;
        }

        private static IDatabase GetRedisDatabase(IServiceProvider container)
        {
            var connectionMultiplexer = container.GetRequiredService<ConnectionMultiplexer>();

            return connectionMultiplexer.GetDatabase();
        }
    }
}