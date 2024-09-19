using Microsoft.Extensions.DependencyInjection;
using TodoService.Business.Services;

namespace TodoService.Business
{
    public static class ServiceRegistration
    {
        public static void RegisterBusinessServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ITodoStorage, TodoStorage>();
        }
    }
}
