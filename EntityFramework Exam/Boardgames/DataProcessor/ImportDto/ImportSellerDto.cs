using System.ComponentModel.DataAnnotations;
using Boardgames.Data.Models;

namespace Boardgames.DataProcessor.ImportDto;

public class ImportSellerDto
{
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

    public virtual ICollection<int> Boardgames { get; set; }
}