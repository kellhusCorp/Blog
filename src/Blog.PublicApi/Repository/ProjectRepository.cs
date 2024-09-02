using Blog.Domain;
using Blog.Domain.Entities;
using Blog.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MyBlogOnCore.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly BlogDbContext _dbContext;

    public ProjectRepository(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Project>> GetAll()
    {
        return await _dbContext.Projects.AsNoTracking().ToListAsync();
    }

    public async Task<Project> CreateOrUpdate(Project entity)
    {
        if (entity.ProjectId > 0)
        {
            _dbContext.Projects.Update(entity);
        }
        else
        {
            await _dbContext.Projects.AddAsync(entity);
        }

        await _dbContext.SaveChangesAsync();

        return entity;
    }
}