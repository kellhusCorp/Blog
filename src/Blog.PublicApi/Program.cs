using System.Globalization;
using Blog.Application.Contexts;
using Blog.Application.Extensions;
using Blog.Application.Factories;
using Blog.Application.Profiles;
using Blog.Application.Settings;
using Blog.Application.UseCases.GetPages;
using Blog.BLL.Handlers;
using Blog.Infrastructure.Contexts;
using Blog.PublicApi.Extensions;
using Blog.PublicApi.Profiles;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyBlogOnCore.Middlewares;
using MyBlogOnCore.Options;
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
builder.Services.AddSingleton<IFileProviderFactory, FileProviderFactory>(serviceProvider =>
{
    StorageServicesSettings settings = serviceProvider.GetRequiredService<IOptions<StorageServicesSettings>>().Value;
    return new FileProviderFactory(builder.Environment.ContentRootPath, settings);
});
builder.Services.AddDbContext<IDbContext, BlogDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity();

builder.Services.AddLocalization();

builder.Services.AddControllersWithViews()
    .AddViewLocalization();

builder.Services.AddDomainServices();

builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(typeof(GetPagesMetadataQuery).Assembly,
    typeof(IncrementVisitsNumberHandler).Assembly));

builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddMaps(typeof(PageProfile).Assembly, typeof(DefaultProfile).Assembly);
});

builder.Services.AddLegacyHandlers();
builder.Services.AddApplicationServices();

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

CultureInfo[] supportedCultures = { new CultureInfo("ru"), new CultureInfo("en") };

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