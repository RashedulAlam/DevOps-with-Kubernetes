using Microsoft.Extensions.DependencyInjection;
using TodoService.Infrastructure.Repository;

namespace TodoService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
