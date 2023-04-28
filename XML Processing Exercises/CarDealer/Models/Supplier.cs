using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealer.Common;

namespace CarDealer.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(EntityValidations.MaxSupplierNameLength)]
        public string Name { get; set; } = null!;

        public bool IsImporter { get; set; }
        
        [InverseProperty(nameof(Supplier))]
        public ICollection<Part> Parts { get; set; } = new List<Part>();
    }
}
