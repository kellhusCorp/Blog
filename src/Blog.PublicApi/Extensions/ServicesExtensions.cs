using Blog.BLL.Handlers;

namespace Blog.PublicApi.Extensions
{
    public static class ServicesExtensions
    {
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