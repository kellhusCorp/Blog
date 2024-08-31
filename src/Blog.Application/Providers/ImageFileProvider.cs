using Blog.BLL.Providers;
using Microsoft.Extensions.Options;

namespace Blog.Application.Providers;

public class ImageFileProvider : BaseFileProvider, IImageFileProvider
{
    public ImageFileProvider(
        IHostEnvironment hostEnvironment,
        IOptionsMonitor<StorageServicesSettings> servicesSettings)
        : base(hostEnvironment, servicesSettings.CurrentValue.InvariantImageRootDirectory)
    {
    }
}