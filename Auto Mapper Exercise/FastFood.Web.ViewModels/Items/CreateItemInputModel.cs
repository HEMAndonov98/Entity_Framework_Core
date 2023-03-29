using System.ComponentModel.DataAnnotations;
using Common;

namespace FastFood.Web.ViewModels.Items
{
    public class CreateItemInputModel
    {
        [MinLength(ViewModelValidations.ItemNameMinLength)]
        [MaxLength(ViewModelValidations.ItemNameMaxLength)]
        public string Name { get; set; } = null!;

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        
        public int CategoryId { get; set; }
    }
}
