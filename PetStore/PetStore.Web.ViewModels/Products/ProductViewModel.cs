namespace PetStore.Web.ViewModels.Products;

public class ProductViewModel
{
    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Category { get; set; } = null!;

    public Guid ProductId { get; set; }
}