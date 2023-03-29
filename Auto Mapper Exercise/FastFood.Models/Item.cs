using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace FastFood.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Item
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(EntityValidations.ItemNameMaxLength)]
        public string? Name { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; } = null!;

        public decimal Price { get; set; }

        [InverseProperty(nameof(Item))]
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}