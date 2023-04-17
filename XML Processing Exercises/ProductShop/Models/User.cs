﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductShop.Models
{
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.ProductsSold = new List<Product>();
            this.ProductsBought = new List<Product>();
        }

        [Key]
        public int Id { get; set; }

        public string? FirstName { get; set; }

        [Required]
        public string LastName { get; set; } = null!;

        public int? Age { get; set; }

        [InverseProperty(nameof(User))]
        public ICollection<Product> ProductsSold { get; set; }
        [InverseProperty(nameof(User))]
        public ICollection<Product> ProductsBought { get; set; }
    }
}