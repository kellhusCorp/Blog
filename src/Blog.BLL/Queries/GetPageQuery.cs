using Blog.BLL.Dtos;
using MediatR;

namespace Blog.BLL.Queries
{
    public class GetPageQuery : IRequest<IEnumerable<PageDto>>
    {
        public string PageName { get; set; }
    }
}