using System.ComponentModel.DataAnnotations;
using Blog.Data.Common.Constraints;
using Blog.Web.Common.LoginViewModelErorrMessages;

namespace Blog.Web.ViewModels.Identity;

/// <summary>
/// View model for logging in 
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// User name
    /// </summary>
    [Display(Name = "UserName")]
    [Required(AllowEmptyStrings = false,
        ErrorMessage = LogInErrorMessages.RequiredFieldError)]
    [StringLength(ApplicationUserConstraints.UserNameMaxLength,
        MinimumLength = ApplicationUserConstraints.UserNameMinLength,
        ErrorMessage = LogInErrorMessages.InvalidFieldLength)]
    public string UserName { get; set; } = null!;

    /// <summary>
    /// User password
    /// </summary>
    [Display(Name = "Password")]
    [Required(AllowEmptyStrings = false,
        ErrorMessage = LogInErrorMessages.RequiredFieldError)]
    [StringLength(ApplicationUserConstraints.PasswordMaxLength,
        MinimumLength = ApplicationUserConstraints.PasswordMinLength,
        ErrorMessage = LogInErrorMessages.InvalidFieldLength)]
    public string Password { get; set; }
}