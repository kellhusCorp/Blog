using Blog.Application.Settings;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

namespace Blog.PublicApi.Extensions
{
    public static class StaticFilesExtensions
    {
        /// <summary>
        /// Configures using static files middleware.
        /// </summary>
        /// <param name="app">Instance of <see cref="IApplicationBuilder"/>.</param>
        public static void ConfigureStaticFiles(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var storageServicesSettings = scope.ServiceProvider.GetService<IOptionsMonitor<StorageServicesSettings>>();
            var environment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var correctlyPathToImages = storageServicesSettings.CurrentValue.InvariantImageRootDirectory;
            var correctlyPathToFiles = storageServicesSettings.CurrentValue.InvariantFilesRootDirectory;
            InternalUseStaticFiles(correctlyPathToImages, storageServicesSettings.CurrentValue.ImagesRootDirectory);
            InternalUseStaticFiles(correctlyPathToFiles, storageServicesSettings.CurrentValue.FilesRootDirectory);

            void InternalUseStaticFiles(string relativePath, string requestPath)
            {
                var absolutePath = Path.Combine(environment.ContentRootPath, relativePath);
                if (!Directory.Exists(absolutePath))
                {
                    Directory.CreateDirectory(absolutePath);
                }

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(absolutePath), RequestPath = requestPath
                });
            }
        }
    }
}