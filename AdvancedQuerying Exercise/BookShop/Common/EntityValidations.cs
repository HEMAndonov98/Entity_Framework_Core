namespace Common;

public static class EntityValidations
{
    //Author
    public const int AuthorFirstNameLength = 50;

    public const string AuthorFirstNameSqlType = "NVARCHAR";

    public const int AuthorLastNameLength = 50;

    public const string AuthorLastNameSqlType = "NVARCHAR";
    
    //Book

    public const int BookTitleLength = 50;

    public const string BookTitleSqlType = "NVARCHAR";

    public const int BookDescriptionLength = 1000;

    public const string BookDescriptionSqlType = "NVARCHAR";

    public const string BookPriceSqlType = "DECIMAL(8, 2)";
    
    //Category

    public const int CategoryNameLength = 50;

    public const string CategoryNameSqlType = "NVARCHAR";
}