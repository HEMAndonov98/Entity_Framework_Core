using System.ComponentModel.DataAnnotations;
using Blog.Data.Common.Constraints;
using Blog.Web.Common.RegisterViewModelErrorMessages;

namespace Blog.Web.ViewModels.Identity;

public class RegisterViewModel
{
    [Display(Name = "UserName")]
    [Required]
    [StringLength(ApplicationUserConstraints.UserNameMaxLength,
        MinimumLength = ApplicationUserConstraints.UserNameMinLength,
        ErrorMessage = RegisterErrorMessages.IncorrectFieldLengthMessage)]
    public string UserName { get; set; } = null!;

    [Display(Name = "Email")]
    [Required]
    [StringLength(ApplicationUserConstraints.EmailMaxLength,
        MinimumLength = ApplicationUserConstraints.EmailMinLength,
        ErrorMessage = RegisterErrorMessages.IncorrectFieldLengthMessage)] 
    public string Email { get; set; } = null!;

    [Display(Name = "Password")]
    [Required]
    [StringLength(ApplicationUserConstraints.PasswordMaxLength,
        MinimumLength = ApplicationUserConstraints.PasswordMinLength,
        ErrorMessage = RegisterErrorMessages.IncorrectFieldLengthMessage)]
    public string Password { get; set; } = null!;

    [Display(Name = "Password Confirmation")]
    [Required]
    public string ConfirmPassword { get; set; } = null!;
}