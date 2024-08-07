using Blog.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogOnCore.Repository;

namespace MyBlogOnCore.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectRepository repository;

    public ProjectsController(IProjectRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var projects = await repository.GetAll();
        return View(projects);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("CategoryName, Title, Description, LinkToSite, LinkToImage")]Project project)
    {
        if (ModelState.IsValid)
        {
            await repository.CreateOrUpdate(project);
            return RedirectToAction(nameof(Index));
        }

        return View();
    }
}