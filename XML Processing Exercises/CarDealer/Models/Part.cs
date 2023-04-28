using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealer.Common;

namespace CarDealer.Models
{
    public class Part
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(EntityValidations.MaxPartNameLength)]
        public string Name { get; set; } = null!; 
        
        [Column(TypeName = EntityValidations.MaxPartPrice)]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int SupplierId { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;

        [InverseProperty(nameof(Part))]
        public ICollection<PartCar> PartsCars { get; set; } = new List<PartCar>();
    }
}
