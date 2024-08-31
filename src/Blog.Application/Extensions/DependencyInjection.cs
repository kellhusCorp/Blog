using System.Reflection;
using Blog.Application.Factories;
using Blog.Application.Services;
using Blog.Application.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Blog.Application.Extensions
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var servicesToInject = assembly.GetTypes()
                .Where(x => x.IsAssignableTo(typeof(IApplicationService)) && !x.IsAbstract);
            foreach (var service in servicesToInject)
            {
                services.AddScoped(service);
            }
            
            services.AddScoped<IDateTimeProvider, DefaultDateTimeProvider>();
            services.AddScoped<IImageStorageService, ImageStorageService>();
        }
    }
}