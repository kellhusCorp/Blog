using Blog.BLL.Handlers;
using Blog.BLL.Providers;
using Blog.BLL.Repositories;
using Blog.BLL.Services;

namespace MyBlogOnCore.Extensions
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
            services.AddTransient<IImageFileProvider, ImageFileProvider>();
            services.AddTransient<IBlogFileProvider, BlogFileProvider>();
            services.AddScoped<IPagesRepository, PagesRepository>();
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