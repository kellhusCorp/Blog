﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using MyBlogOnCore.Localization;

namespace MyBlogOnCore.Domain;

public class User : IdentityUser
{
    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.FirstName), ResourceType = typeof(Resources))]
    public string FirstName { get; set; }

    [StringLength(100, ErrorMessageResourceName = nameof(Resources.Validation_MaxLength), ErrorMessageResourceType = typeof(Resources))]
    [Required(ErrorMessageResourceName = nameof(Resources.Validation_Required), ErrorMessageResourceType = typeof(Resources))]
    [Display(Name = nameof(Resources.LastName), ResourceType = typeof(Resources))]
    public string LastName { get; set; }

    public virtual ICollection<Blog>? Blogs { get; set; }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}