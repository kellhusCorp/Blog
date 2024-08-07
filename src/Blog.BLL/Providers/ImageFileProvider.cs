using Blog.BLL.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Blog.BLL.Providers;

public class ImageFileProvider : BaseFileProvider, IImageFileProvider
{
    public ImageFileProvider(
        IHostEnvironment hostEnvironment,
        IOptionsMonitor<StorageServicesSettings> servicesSettings)
        : base(hostEnvironment, servicesSettings.CurrentValue.InvariantImageRootDirectory)
    {
    }
}