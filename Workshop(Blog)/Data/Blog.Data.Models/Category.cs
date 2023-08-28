using Blog.Data.Common.Models;

namespace Blog.Data.Models;

public class Category : BaseModel<int>
{
    public string Name { get; set; }
}