namespace Blog.Data.Common.Constraints;

/// <summary>
/// Constraints for a single user
/// </summary>
public static class ApplicationUserConstraints
{
    public const int UserNameMinLength = 5;
    public const int UserNameMaxLength = 20;

    public const int EmailMinLength = 10;
    public const int EmailMaxLength = 50;

    public const int PasswordMinLength = 5;
    public const int PasswordMaxLength = 20;
}