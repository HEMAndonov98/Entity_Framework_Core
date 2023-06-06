namespace PetStore.Common;

public static class ProductInputValidationConstants
{
    public const int NameMaxLength = 50;
    public const int NameMinLength = 3;
    
    public const string InvalidNameLengthErrorMessage = "Name must be between 5 and 50 characters long";
}