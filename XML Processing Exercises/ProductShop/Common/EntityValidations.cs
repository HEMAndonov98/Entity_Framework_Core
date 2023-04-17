namespace ProductShop.Common;

public static class EntityValidations
{
    
    //User
    
    public const int UserFirstNameMaxLength = 50;

    public const int UserLastNameMaxLength = 50;
    
    //Product

    public const int ProductNameMaxLength = 200;

    public const string ProductPrice = "DEC(16, 2)";
    
    
    //Category

    public const int CategoryNameMaxLength = 30;
}