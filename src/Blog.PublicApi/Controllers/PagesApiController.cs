using Blog.Application.UseCases.GetPages;
using Blog.PublicApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.PublicApi.Controllers
{
    [ApiController]
    [Route("/api/pages")]
    public class PagesApiController(IMediator mediator) : ControllerBase
    {
        /// <summary>
        /// Gets pages metadata for navbar. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetMetadata(CancellationToken cancellationToken) 
            => (await mediator.Send(new GetPagesMetadataQuery(), cancellationToken)).AsOk();
    }
}