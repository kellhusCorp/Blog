using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MyBlogOnCore.BLL.Settings;

namespace MyBlogOnCore.BLL.Providers;

public class ImageFileProvider : BaseFileProvider, IImageFileProvider
{
    public ImageFileProvider(
        IHostEnvironment hostEnvironment,
        IOptionsMonitor<StorageServicesSettings> servicesSettings)
        : base(hostEnvironment, servicesSettings.CurrentValue.InvariantImageRootDirectory)
    {
    }
}