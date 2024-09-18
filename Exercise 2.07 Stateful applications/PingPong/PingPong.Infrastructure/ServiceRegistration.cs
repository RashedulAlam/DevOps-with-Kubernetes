using Microsoft.Extensions.DependencyInjection;
using PingPong.Infrastructure.Repository;

namespace PingPong.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
