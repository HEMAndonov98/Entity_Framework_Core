using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PetStore.Data;
using PetStore.Data.Common.Repos;
using PetStore.Data.Models;
using PetStore.Web.ViewModels.Categories;
using PetStore.Web.ViewModels.Products;

namespace PetStore.Services.Data;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    private readonly IDeletableEntityRepository<Product> _repository;
    private readonly IMapper _mapper;
    
    public ProductService(ApplicationDbContext context, IDeletableEntityRepository<Product> repository, IMapper mapper)
    {
        this._context = context;
        this._repository = repository;
        this._mapper = mapper;
    }
    
    public async Task CreateAsync(CreateProductInputModel inputModel)
    {
        Product newProduct = this._mapper.Map<Product>(inputModel);

        await this._repository.AddAsync(newProduct);
        await this._repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
    {
        return await this._repository.AllAsNoTracking()
            .ProjectTo<ProductViewModel>(this._mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    public async Task<IEnumerable<CategoryListViewModel>> GetAllCategoriesAsync()
    {
        return await this._context.Categories
            .AsNoTracking()
            .ProjectTo<CategoryListViewModel>(this._mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    public async Task DeleteAsync(string id)
    {
        Product? product = await this._context.Products
            .FindAsync(id);

        if (product != null)
        { 
            this._repository.Delete(product);
            await this._repository.SaveChangesAsync();
        }
    }

    public async Task<EditProductModel> GetProduct(string id)
    {
        Product? product = await this._context
            .Products
            .FindAsync(id);

        if (product == null)
            throw new InvalidOperationException();

        var editModel = this._mapper.Map<EditProductModel>(product);
        
        return editModel;
    }

    public async Task Edit(EditProductModel model)
    {
        Product product = this._mapper.Map<Product>(model);
        
        this._repository.Update(product);
        await this._repository.SaveChangesAsync();
    }
}