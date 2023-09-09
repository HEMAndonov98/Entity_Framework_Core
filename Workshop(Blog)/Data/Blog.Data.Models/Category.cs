namespace Blog.Data.Models;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Blog.Data.Common.Constraints;
using Blog.Data.Common.Models;

public class Category : BaseModel<int>
{
    [MaxLength(CategoryConstraints.NameMaxLength)]
    [Required]
    public string Name { get; set; } = null!;

    public ICollection<Article> Articles { get; set; }
}