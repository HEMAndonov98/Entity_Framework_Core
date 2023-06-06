using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PetStore.Data;
using PetStore.Data.Common.Repos;
using PetStore.Data.Models;
using PetStore.Web.ViewModels.Categories;

namespace PetStore.Services.Data;


public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IDeletableEntityRepository<Category> _repository;
    private readonly IMapper _mapper;

    public CategoryService(IDeletableEntityRepository<Category> repository, IMapper mapper, ApplicationDbContext context)
    {
        this._context = context;
        this._repository = repository;
        this._mapper = mapper;
    }
    
    public async Task Create(CreateCategoryInputModel inputModel)
    {
        Category newCategory = _mapper.Map<Category>(inputModel);
        await this._repository.AddAsync(newCategory);
        await this._repository.SaveChangesAsync();
    }

    public async Task<IEnumerable<CategoryListViewModel>> GetAll()
    {
        return await this._repository.AllAsNoTracking()
            .ProjectTo<CategoryListViewModel>(this._mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    public async Task Delete(int id)
    {

        var item = await this._context.Categories.FindAsync(id);
        this._repository.Delete(item);
        await this._repository.SaveChangesAsync();
    }

    public async Task Edit(CategoryListViewModel viewModel)
    {
        var item = this._mapper.Map<Category>(viewModel);
        this._repository.Update(item);
        await this._repository.SaveChangesAsync();
    }

    public async Task<CategoryListViewModel> Get(int id)
    {
        var item = await this._context.Categories
            .FindAsync(id);

        return _mapper.Map<CategoryListViewModel>(item);
    }
}