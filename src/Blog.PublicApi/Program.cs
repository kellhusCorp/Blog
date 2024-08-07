using System.Globalization;
using Blog.BLL.Handlers;
using Blog.BLL.Settings;
using Blog.Infrastructure.Contexts;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MyBlogOnCore.Extensions;
using MyBlogOnCore.Middlewares;
using MyBlogOnCore.Options;
using MyBlogOnCore.Profiles;
using NLog;
using NLog.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
    })
    .UseNLog();

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<BlogSettings>(builder.Configuration.GetSection("BlogSettings"));
builder.Services.Configure<StorageServicesSettings>(builder.Configuration.GetSection("StorageServicesSettings"));

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity();

builder.Services.AddLocalization();

builder.Services.AddControllersWithViews()
    .AddViewLocalization();

builder.Services.AddDomainServices();

builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(typeof(AddOrUpdateBlogHandler).Assembly));

builder.Services.AddAutoMapper(expression =>
{
    expression.AddProfile<PageProfile>();
});

builder.Services.AddLegacyHandlers();

WebApplication app = builder.Build();

app.ConfigureStaticFiles();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.ApplyMigration<BlogDbContext>();

app.CreateAdminUser(new AdminUserOptions
{
    Email = app.Configuration.GetSection("Credentials")["AdminEmail"],
    FirstName = "Admin",
    LastName = "Admin",
    IsConfirmed = true,
    Password = app.Configuration.GetSection("Credentials")["AdminPassword"],
    RoleNames = new[] { "Admin" }
});

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

CultureInfo[] supportedCultures = new[] { new CultureInfo("ru"), new CultureInfo("en") };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(supportedCultures[0]),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();