using System.Diagnostics.CodeAnalysis;
using MessagingService.Services;
using MessagingService.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingService.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }
    }
}