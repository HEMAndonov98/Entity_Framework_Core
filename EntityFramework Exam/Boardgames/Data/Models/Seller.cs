using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models;

public class Seller
{
    public Seller()
    {
        this.BoardgamesSellers = new HashSet<BoardgameSeller>();
    }
    
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    [MinLength(5)]
    public string Name { get; set; } = null!;

    [Required]
    [MinLength(2)]
    [MaxLength(30)]
    public string Address { get; set; } = null!;

    [Required] [MaxLength(40)] 
    public string Country { get; set; } = null!;

    [Required]
    [RegularExpression(@"^www\.[a-zA-Z0-9\-]+\.com$")]
    public string Website { get; set; } = null!;

    public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; }
}
//
// Id – integer, Primary Key
// Name – text with length [5…20] (required)
//     Address – text with length [2…30] (required)
// Country – text (required)
// Website – a string (required). First four characters are "www.", followed by upper and lower letters, digits or '-' and the last three characters are ".com".
//     BoardgamesSellers – collection of type BoardgameSeller
