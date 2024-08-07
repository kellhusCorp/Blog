using Blog.BLL.Dtos;
using Blog.Domain;
using MediatR;

namespace Blog.BLL.Queries
{
    public class GetPagesMetadataQuery : IRequest<Result<IEnumerable<PageMetadataDto>>>
    {
        
    }
}