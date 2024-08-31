using Blog.Application.Settings;
using Microsoft.Extensions.Options;

namespace Blog.Application.Providers;

public class ImageFileProvider : BaseFileProvider, IImageFileProvider
{
    public ImageFileProvider(
        string pathToContentRootDirectory,
        StorageServicesSettings servicesSettings)
        : base(pathToContentRootDirectory, servicesSettings.InvariantImageRootDirectory)
    {
    }
}