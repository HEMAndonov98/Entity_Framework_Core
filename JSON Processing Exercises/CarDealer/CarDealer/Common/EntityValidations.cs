namespace CarDealer.Common;

public static class EntityValidations
{
    //Part
    public const string PartPrice = "DECIMAL(16,2)";

    public const int PartNameMaxLength = 80;
    
    //Car
    public const int CarMakeMaxLength = 40;

    public const int CarModelMaxLength = 40;
    
    //Customer
    public const int CustomerNameMaxLength = 80;
    
    //Sale
    public const string SaleDiscountRange = "DECIMAL(6,2)";
    
    //Supplier
    public const int SupplierNameMaxLength = 100;
}