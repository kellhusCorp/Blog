using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogOnCore.BLL.Dtos;
using MyBlogOnCore.BLL.Queries;
using PetBlog.Infrastructure.Types;

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
            OperationResult<IEnumerable<PageMetadataDto>> result = await _mediator.Send(new GetPagesMetadataQuery());
            
            if (!result.Succeeded)
            {
                return NotFound();
            }
            
            return Ok(result.Value);
        }
    }
}