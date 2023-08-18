namespace Blog.Data.Common.Constraints;

/// <summary>
/// Constraints for Article entity model
/// </summary>
public static class ArticleConstraints
{
    public const int TitleMinLength = 10;
    public const int TitleMaxLength = 50;

    public const int ContentMinLength = 50;
    public const int ContentMaxLength = 1500;
}