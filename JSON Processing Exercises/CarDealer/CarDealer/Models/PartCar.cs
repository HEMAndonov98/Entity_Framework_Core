using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    public class PartCar
    {
        [Key]
        [ForeignKey(nameof(Part))]
        public int PartId { get; set; }
        public Part Part { get; set; } = null!; 

        [Key]
        [ForeignKey(nameof(Car))]
        public int CarId { get; set; }
        public Car Car { get; set; } = null!; 
    }
}
