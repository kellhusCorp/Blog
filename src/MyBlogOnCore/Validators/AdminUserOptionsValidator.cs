using System.ComponentModel.DataAnnotations;
using MyBlogOnCore.Options;

namespace MyBlogOnCore.Validators;

public class AdminUserOptionsValidator
{
    public IDictionary<string, string> Errors { get; }

    public AdminUserOptionsValidator()
    {
        Errors = new Dictionary<string, string>();
    }

    public AdminUserValidatorResult Validate(AdminUserOptions? options)
    {
        if (options == null)
        {
            Errors.Add(nameof(options), "Options is null");
        }

        if (string.IsNullOrEmpty(options.FirstName))
        {
            Errors.Add(nameof(options.FirstName), "Firstname is empty");
        }

        if (string.IsNullOrEmpty(options.LastName))
        {
            Errors.Add(nameof(options.LastName), "Lastname is empty");
        }

        if (string.IsNullOrEmpty(options.Email) || !new EmailAddressAttribute().IsValid(options.Email))
        {
            Errors.Add(nameof(options.Email), "Email isn't correct");
        }

        if (string.IsNullOrEmpty(options.Password))
        {
            Errors.Add(nameof(options.Password), "Password is empty");
        }

        if (!options.RoleNames.Any())
        {
            Errors.Add(nameof(options.RoleNames), "Rolenames is empty");
        }

        return Errors.Any() ? 
            AdminUserValidatorResult.Error : 
            AdminUserValidatorResult.Success;
    }
    
    public enum AdminUserValidatorResult
    {
        Success,
        Error
    }
}