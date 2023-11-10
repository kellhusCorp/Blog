using Microsoft.AspNetCore.Identity;
using MyBlogOnCore.DataSource.Contexts;
using MyBlogOnCore.Domain;

namespace MyBlogOnCore.Extensions
{
    public static class IdentityExtensions
    {
        /// <summary>
        /// Добавляет необходимые сервисы для работы с Identity.
        /// </summary>
        /// <param name="services">Коллекция сервисов <see cref="IServiceCollection"/>.</param>
        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<BlogDbContext>();
        }
    }
}