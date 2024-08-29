using System.Reflection;
using Blog.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Application.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var servicesToInject = assembly.GetTypes().Where(x => x.IsAssignableTo(typeof(IApplicationService)) && !x.IsAbstract);
            foreach (var service in servicesToInject)
            {
                services.AddScoped(service);
            }
            
            services.AddScoped<IDateTimeProvider, DefaultDateTimeProvider>();
        }
    }
}