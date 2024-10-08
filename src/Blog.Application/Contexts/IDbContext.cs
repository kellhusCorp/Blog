﻿using Blog.Domain;
using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.Application.Contexts
{
    public interface IDbContext
    {
        public DbSet<Post> Posts { get; set; }
        
        public DbSet<PostFile> PostFiles { get; set; }
        
        public DbSet<Comment> Comments { get; set; }
        
        public DbSet<Image> Images { get; set; }
        
        public DbSet<Tag> Tags { get; set; }
        
        public DbSet<TagAssignment> TagAssignments { get; set; }
        
        public DbSet<Page> Pages { get; set; }
        
        public DbSet<Project> Projects { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        
        // what method I should add here that executes update from EF? 
    }
}