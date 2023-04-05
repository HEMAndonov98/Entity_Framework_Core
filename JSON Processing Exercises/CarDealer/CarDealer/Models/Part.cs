using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealer.Common;

namespace CarDealer.Models
{
    using static Common.EntityValidations;

    public class Part
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(EntityValidations.PartNameMaxLength)]
        public string Name { get; set; } = null!; 

        [Column(TypeName = $"{EntityValidations.PartPrice}")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [ForeignKey(nameof(Supplier))]
        public int SupplierId { get; set; }

        public Supplier Supplier { get; set; } = null!;

        [InverseProperty(nameof(Part))]
        public ICollection<PartCar> PartsCars { get; set; } = new List<PartCar>();
    }
}
