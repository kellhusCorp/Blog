using Blog.Domain.Entities;
using Blog.Infrastructure.Contexts;
using Blog.PublicApi.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.Exceptions;
using MyBlogOnCore.Options;

namespace Blog.PublicApi.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void CreateAdminUser(
        this IApplicationBuilder app,
        AdminUserOptions? options)
    {
        var validator = new AdminUserOptionsValidator();
        
        var validateResult = validator.Validate(options);

        if (validateResult == AdminUserOptionsValidator.AdminUserValidatorResult.Error)
        {
            throw new AdminUserValidatorException("There are validation errors, see Errors for details", validator.Errors);
        }

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            if (roleManager == null)
            {
                throw new ArgumentException("Rolemanager not registered");
            }

            if (userManager == null)
            {
                throw new ArgumentException("Usermanager not registered");
            }
            
            foreach (var role in options.RoleNames)
            {
                var roleExist = roleManager.RoleExistsAsync(role).GetAwaiter().GetResult();
                if (!roleExist)
                {
                    roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                }
            }

            var user = userManager.FindByEmailAsync(options.Email).GetAwaiter().GetResult();

            if (user == null)
            {
                var newUser = new User
                {
                    FirstName = options.FirstName,
                    LastName = options.LastName,
                    UserName = options.Email,
                    Email = options.Email,
                    EmailConfirmed = options.IsConfirmed
                };

                var createAdminUser = userManager.CreateAsync(newUser, options.Password).GetAwaiter().GetResult();

                if (createAdminUser.Succeeded)
                {
                    foreach (var roleName in options.RoleNames)
                    {
                        userManager.AddToRoleAsync(newUser, roleName).GetAwaiter().GetResult();
                    }
                }
            }
        }
    }

    public static void ApplyMigration<TContext>(
        this IApplicationBuilder app)
    where TContext : IMigratoryContext
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<TContext>();
            
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}