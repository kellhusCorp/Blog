using MediatR;
using MyBlogOnCore.BLL.Dtos;
using PetBlog.Infrastructure.Types;

namespace MyBlogOnCore.BLL.Queries
{
    public class GetPageQuery : IRequest<OperationResult<PageDto>>
    {
        public string PageName { get; set; }
    }
}