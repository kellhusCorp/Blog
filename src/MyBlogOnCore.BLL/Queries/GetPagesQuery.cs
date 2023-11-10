using MediatR;
using MyBlogOnCore.BLL.Dtos;
using PetBlog.Infrastructure.Types;

namespace MyBlogOnCore.BLL.Queries
{
    public class GetPagesQuery : IRequest<OperationResult<IEnumerable<PageDto>>>
    {
        
    }
}