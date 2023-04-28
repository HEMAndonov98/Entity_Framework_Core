namespace CarDealer.Common;

public static class EntityValidations
{
    //Car
    public const int MaxCarMakeLength = 60;

    public const int MaxCarModelLength = 80;
    
    //Part
    public const int MaxPartNameLength = 70;
    public const string MaxPartPrice = "DEC(18,2)";
    
    //Sale
    public const string MaxSaleDiscount = "DEC(5,2)";
    
    //Customer
    public const int MaxCustomerNameLength = 120;
    
    //Supplier
    public const int MaxSupplierNameLength = 60;
}