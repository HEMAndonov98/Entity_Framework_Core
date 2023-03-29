using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(EntityValidations.EmployeeNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        public int Age { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }

        [Required]
        public Position Position { get; set; }

        [InverseProperty(nameof(Employee))]
        public ICollection<Order> Orders { get; set; } = new List<Order>(); 
    }
}