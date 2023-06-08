using System.ComponentModel.DataAnnotations;
using PetStore.Common;

namespace PetStore.Web.ViewModels.Products;

public class EditProductModel
{
    public string Id { get; set; } = null!;

    [Required]
    [StringLength(ProductInputValidationConstants.NameMaxLength,
        MinimumLength = ProductInputValidationConstants.NameMinLength,
        ErrorMessage = ProductInputValidationConstants.InvalidNameLengthErrorMessage)]
    public string Name { get; set; } = null!;

    [Required]
    public decimal Price { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
}