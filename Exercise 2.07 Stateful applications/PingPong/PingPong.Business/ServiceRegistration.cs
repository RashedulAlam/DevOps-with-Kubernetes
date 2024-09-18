using Microsoft.Extensions.DependencyInjection;
using PingPong.Business.Services;
using PingPong.Infrastructure.Repository;

namespace PingPong.Business
{
    public static class ServiceRegistration
    {
        public static void RegisterBusinessServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPingStorage, PingStorage>();
        }
    }
}
