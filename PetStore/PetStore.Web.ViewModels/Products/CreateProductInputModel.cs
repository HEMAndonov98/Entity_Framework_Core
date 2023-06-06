using System.ComponentModel.DataAnnotations;
using PetStore.Common;
using PetStore.Web.ViewModels.Categories;


namespace PetStore.Web.ViewModels.Products;

public class CreateProductInputModel
{
    [Required]
    [StringLength(ProductInputValidationConstants.NameMaxLength,
        MinimumLength = ProductInputValidationConstants.NameMinLength,
        ErrorMessage = ProductInputValidationConstants.InvalidNameLengthErrorMessage)]
    public string Name { get; set; } = null!;
    [Required]
    public string Price { get; set; }

    [Required]
    public int CategoryId { get; set; }

}