using Blog.Application.UseCases.GetPages;
using Blog.Domain;
using Blog.PublicApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyBlogOnCore.Controllers
{
    [ApiController]
    [Route("/api/pages")]
    public class PagesApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PagesApiController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Gets pages metadata for navbar. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMetadata(CancellationToken cancellationToken)
        {
            Result<IEnumerable<PageMetadataDto>> result = await _mediator.Send(new GetPagesMetadataQuery(), cancellationToken);
            
            return result.AsOk();
        }
    }
}