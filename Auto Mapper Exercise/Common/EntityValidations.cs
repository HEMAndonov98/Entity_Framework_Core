namespace Common;

public static class EntityValidations
{
    //Positions 
    public const int PositionNameMaxLength = 30;

    //Employees
    public const int EmployeeNameMaxLength = 30;

    public const int EmployeeMinAge = 15;
    public const int EmployeeMaxAge = 80;
    
    public const int EmployeeAddressMinLength = 3;
    public const int EmployeeAddressMaxLength = 30;
    
    //Items

    public const int ItemNameMaxLength = 30;

    public const int ItemPriceDecimal = 32;
    public const int ItemPriceDecimalDigits = 2;
}