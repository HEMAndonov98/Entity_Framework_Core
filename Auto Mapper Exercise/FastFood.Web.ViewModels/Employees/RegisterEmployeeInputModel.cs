using System.ComponentModel.DataAnnotations;
using Common;

namespace FastFood.Web.ViewModels.Employees
{
    public class RegisterEmployeeInputModel
    {
        [MinLength(ViewModelValidations.EmployeeNameMinLength)]
        [MaxLength(ViewModelValidations.EmployeeNameMaxLength)]
        public string Name { get; set; }  = null!;
        
        [Range(ViewModelValidations.EmployeeMinAge, ViewModelValidations.EmployeeMaxAge)]
        public int Age { get; set; }

        public int PositionId { get; set; }

        public string PositionName { get; set; } = null!;
        
        [StringLength(ViewModelValidations.EmployeeAddressMaxLength, MinimumLength = ViewModelValidations.EmployeeAddressMinLength)]
        public string Address { get; set; } = null!;
    }
}
