using Blog.Domain;
using MediatR;

namespace Blog.Application.UseCases.GetPages
{
    public class GetPagesHandler : IRequestHandler<GetPagesQuery, Result<IEnumerable<PageDto>>>
    {
        public GetPagesHandler()
        {
            
        }
        
        public Task<Result<IEnumerable<PageDto>>> Handle(GetPagesQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}