using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.UseCases.UpdatePostFile;

public class UpdatePostFileCommand : IRequest<Result<Unit>>
{
    public UpdatePostFileCommand(string fileName, byte[] data, Guid blogId)
    {
        FileName = fileName;
        Data = data;
        BlogId = blogId;
    }

    public string FileName { get; set; }
    
    public byte[] Data { get; set; }
    
    public Guid BlogId { get; set; }
}