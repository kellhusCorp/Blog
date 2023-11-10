using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogOnCore.BLL.Dtos;
using MyBlogOnCore.BLL.Queries;
using PetBlog.Infrastructure.Types;

namespace MyBlogOnCore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PagesController : Controller
    {
        private readonly IMediator _mediator;

        public PagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[controller]/{pageName}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string pageName)
        {
            OperationResult<PageDto> result = await _mediator.Send(new GetPageQuery {PageName = pageName});
            
            if (!result.Succeeded)
            {
                return NotFound();
            }
            
            return View(result.Value);
        }
    }
}