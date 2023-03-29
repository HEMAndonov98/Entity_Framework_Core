using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Web.ViewModels.Items;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.Data;

public class ItemsService : IItemsService
{
    private readonly IMapper mapper;
    private readonly FastFoodContext context;

    public ItemsService(IMapper mapper, FastFoodContext context)
    {
        this.mapper = mapper;
        this.context = context;
    }
    
    public async Task<IList<CreateItemViewModel>> CreateAsync()
    {
        var categories = await this.mapper
            .ProjectTo<CreateItemViewModel>(this.context.Categories)
            .ToListAsync();

        return categories;
    }

    public async Task CreateAsync(CreateItemInputModel inputModel)
    {
        Item item = this.mapper.Map<Item>(inputModel);

        await this.context.Items.AddAsync(item);
        await this.context.SaveChangesAsync();
    }

    public async Task<IList<ItemsAllViewModels>> AllAsync()
        => await this.context.Items
            .Include(i => i.Category)
            .ProjectTo<ItemsAllViewModels>(this.mapper.ConfigurationProvider)
            .ToListAsync();
}