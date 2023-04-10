using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealer.Common;

namespace CarDealer.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        
        [Column(TypeName = $"{EntityValidations.SaleDiscountRange}")]
        public decimal Discount { get; set; }

        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }
        public Car Car { get; set; } = null!;

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!; 
    }
}
