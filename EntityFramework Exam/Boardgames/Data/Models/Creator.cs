using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boardgames.Data.Models;
public class Creator
{
    public Creator()
    {
        this.Boardgames = new HashSet<Boardgame>();
    }
    
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(7)]
    [MinLength(2)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(7)]
    [MinLength(2)]
    public string LastName { get; set; } = null!;

    [InverseProperty(nameof(Creator))]
    public virtual ICollection<Boardgame> Boardgames { get; set; }
}

// Id – integer, Primary Key
// FirstName – text with length [2, 7] (required) 
//     LastName – text with length [2, 7] (required)
// Boardgames – collection of type Boardgame
