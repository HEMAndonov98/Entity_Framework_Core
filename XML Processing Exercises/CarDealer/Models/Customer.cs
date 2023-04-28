using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealer.Common;

namespace CarDealer.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(EntityValidations.MaxCustomerNameLength)]
        public string Name { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public bool IsYoungDriver { get; set; }

        [InverseProperty(nameof(Customer))]
        public ICollection<Sale> Sales { get; set; } = new List<Sale>(); 
    }
}