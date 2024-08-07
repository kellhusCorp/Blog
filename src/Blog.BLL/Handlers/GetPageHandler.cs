using AutoMapper;
using PetBlog.Infrastructure.Types;
using MediatR;
using MyBlogOnCore.BLL.Dtos;
using MyBlogOnCore.BLL.Queries;
using MyBlogOnCore.BLL.Repositories;

namespace MyBlogOnCore.BLL.Handlers
{
    public class GetPageHandler : IRequestHandler<GetPageQuery, OperationResult<PageDto>>
    {
        private readonly IPagesRepository _pagesRepository;
        private readonly IMapper _mapper;

        public GetPageHandler(IPagesRepository pagesRepository, IMapper mapper)
        {
            _pagesRepository = pagesRepository;
            _mapper = mapper;
        }
        
        public async Task<OperationResult<PageDto>> Handle(GetPageQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<PageDto>();
            
            var page = await _pagesRepository.GetByName(request.PageName);

            if (page == null)
            {
                result.AddFailure("Page not found");
                return result;
            }
            
            result.Value = _mapper.Map<PageDto>(page);
            
            return result;
        }
    }
}