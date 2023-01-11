using System.Globalization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using MyBlogOnCore.BLL;
using MyBlogOnCore.BLL.Handlers;
using MyBlogOnCore.BLL.Services;
using MyBlogOnCore.DataSource.Contexts;
using MyBlogOnCore.Domain;
using MyBlogOnCore.Extensions;
using MyBlogOnCore.Options;
using MyBlogOnCore.Repository;
using NLog;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
    })
    .UseNLog();

builder.Services.Configure<BlogSettings>(builder.Configuration.GetSection("BlogSettings"));
builder.Services.Configure<StorageServicesSettings>(builder.Configuration.GetSection("StorageServicesSettings"));

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BlogDbContext>();

builder.Services.AddLocalization();

builder.Services.AddControllersWithViews()
    .AddViewLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddScoped<IBlogService<Blog>, BlogService>();
builder.Services.AddScoped<IImageStorageService, ImageStorageService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IImageFileProvider, ImageFileProvider>();
builder.Services.AddTransient<IBlogFileProvider, BlogFileProvider>();

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
            builder.Services.AddScoped(interfaceType, serviceType);
            break;
        }
    }
}

var app = builder.Build();

AddDirectoryForCustomStaticFiles();

void AddDirectoryForCustomStaticFiles()
{
    using (var scope = app.Services.CreateScope())
    {
        var storageServicesSettings = scope.ServiceProvider.GetService<IOptionsMonitor<StorageServicesSettings>>();

        var correctlyPathToImages = storageServicesSettings.CurrentValue.InvariantImageRootDirectory;

        var correctlyPathToFiles = storageServicesSettings.CurrentValue.InvariantFilesRootDirectory;

        var absolutePathToImages = Path.Combine(builder.Environment.ContentRootPath, correctlyPathToImages);
        Directory.CreateDirectory(absolutePathToImages);
        var absolutePathToFiles = Path.Combine(builder.Environment.ContentRootPath, correctlyPathToFiles);
        Directory.CreateDirectory(absolutePathToFiles);

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(absolutePathToImages),
            RequestPath = storageServicesSettings.CurrentValue.ImagesRootDirectory
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(absolutePathToFiles),
            RequestPath = storageServicesSettings.CurrentValue.FilesRootDirectory
        });
    }
}

app.ApplyMigration<BlogDbContext>();

app.CreateAdminUser(new AdminUserOptions
{
    Email = app.Configuration.GetSection("Credentials")["AdminEmail"],
    FirstName = "Admin",
    LastName = "Admin",
    IsConfirmed = true,
    Password = app.Configuration.GetSection("Credentials")["AdminPassword"],
    RoleNames = new []{ "Admin" }
});

if (app.Environment.IsDevelopment())
    app.UseMigrationsEndPoint();
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

var supportedCultures = new[]
{
    new CultureInfo("ru"),
    new CultureInfo("en")
};

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
