using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealer.Common;

namespace CarDealer.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(EntityValidations.CarMakeMaxLength)]
        public string Make { get; set; } = null!;

        [Required]
        [MaxLength(EntityValidations.CarModelMaxLength)]
        public string Model { get; set; } = null!;

        public int TravelledDistance { get; set; }

        [ForeignKey(nameof(Car))]
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();    

        [InverseProperty(nameof(Car))]
        public ICollection<PartCar> PartsCars { get; set; } = new List<PartCar>();
    }
}
