using System.ComponentModel.DataAnnotations;
using Blog.Domain;
using MediatR;

namespace Blog.Application.UseCases.GetPages
{
    public class GetPagesQuery : IRequest<Result<IEnumerable<PageDto>>>
    {
        [Required]
        public string PageName { get; set; }
    }
}