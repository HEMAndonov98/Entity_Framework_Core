using System.ComponentModel.DataAnnotations;
using PetStore.Common;
using PetStore.Data.Models;

namespace PetStore.Web.ViewModels.Categories;

public class CreateCategoryInputModel
{
    [Required]
    [StringLength(CategoryInputValidationConstants.NameMaxLength,
        MinimumLength = CategoryInputValidationConstants.NameMinLength,
        ErrorMessage = CategoryInputValidationConstants.InvalidNameLengthErrorMessage)]
    public string Name { get; set; } = null!;
    
}