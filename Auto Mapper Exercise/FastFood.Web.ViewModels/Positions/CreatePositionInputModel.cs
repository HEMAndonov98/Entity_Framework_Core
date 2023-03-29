using System.ComponentModel.DataAnnotations;
using Common;

namespace FastFood.Web.ViewModels.Positions
{
    public class CreatePositionInputModel
    {
        [MinLength(ViewModelValidations.PositionNameMinLength)]
        [MaxLength(ViewModelValidations.PositionNameMaxLength)]
        public string PositionName { get; set; } = null!;
    }
}