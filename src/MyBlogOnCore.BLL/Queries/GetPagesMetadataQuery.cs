using MediatR;
using MyBlogOnCore.BLL.Dtos;
using PetBlog.Infrastructure.Types;

namespace MyBlogOnCore.BLL.Queries
{
    public class GetPagesMetadataQuery : IRequest<OperationResult<IEnumerable<PageMetadataDto>>>
    {
        
    }
}