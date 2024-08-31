using Blog.Domain;
using Blog.Domain.Entities;
using MediatR;

namespace Blog.Application.UseCases.GetPages
{
    public class GetPagesMetadataQuery : IRequest<Result<IEnumerable<PageMetadataDto>>>;
}