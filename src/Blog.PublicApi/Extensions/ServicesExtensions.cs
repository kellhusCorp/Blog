using Blog.Application.Factories;
using Blog.Application.Services;
using Blog.Application.Settings;
using Blog.BLL.Handlers;
using Microsoft.Extensions.Options;

namespace Blog.PublicApi.Extensions
{
    public static class ServicesExtensions
    {
        /// <summary>
        /// Добавляет необходиые доменные сервисы.
        /// </summary>
        /// <param name="services">Коллекция сервисов <see cref="IServiceCollection"/>.</param>
        public static void AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IImageStorageService, ImageStorageService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            services.AddSingleton<IFileProviderFactory, FileProviderFactory>(serviceProvider =>
            {
                StorageServicesSettings settings = serviceProvider.GetRequiredService<IOptions<StorageServicesSettings>>().Value;
                IHostEnvironment hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
                return new FileProviderFactory(hostEnvironment.ContentRootPath, settings);
            });
        }

        /// <summary>
        /// Добавляет необходимые обработчики команд.
        /// </summary>
        /// <param name="services">Коллекция сервисов <see cref="IServiceCollection"/>.</param>
        [Obsolete("Use MediatR instead")]
        public static void AddLegacyHandlers(this IServiceCollection services)
        {
            foreach (var serviceType in typeof(ICommandHandler<>).Assembly.GetTypes())
            {
                if (serviceType.IsAbstract || serviceType.IsInterface || serviceType.BaseType == null)
                {
                    continue;
                }

                foreach (var interfaceType in serviceType.GetInterfaces())
                {
                    if (interfaceType.IsGenericType && typeof(ICommandHandler<>).IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
                    {
                        services.AddScoped(interfaceType, serviceType);
                        break;
                    }
                }
            }
        }
    }
}