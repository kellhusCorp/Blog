using Blog.Domain;
using MediatR;

namespace Blog.Application.UseCases.GetPages
{
    public class GetPagesMetadataQuery : IRequest<Result<IEnumerable<PageMetadataDto>>>;
}