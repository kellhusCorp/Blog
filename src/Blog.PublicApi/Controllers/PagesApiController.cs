using Blog.BLL.Dtos;
using Blog.BLL.Queries;
using Blog.Domain;
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
        public async Task<IActionResult> GetPages()
        {
            Result<IEnumerable<PageMetadataDto>> result = await _mediator.Send(new GetPagesMetadataQuery());
            
            if (!result.IsSuccessful)
            {
                return NotFound();
            }
            
            return Ok(result.Value);
        }
    }
}