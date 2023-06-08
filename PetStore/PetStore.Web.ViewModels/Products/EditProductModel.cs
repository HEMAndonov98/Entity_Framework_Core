namespace PetStore.Web.ViewModels.Products;

public class EditProductModel
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int CategoryId { get; set; }
}