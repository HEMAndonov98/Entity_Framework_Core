using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Web.ViewModels.Positions;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.Data;

public class PositionsService : IPositionsService
{
    private readonly IMapper mapper;
    private readonly FastFoodContext context;

    public PositionsService(IMapper mapper, FastFoodContext context)
    {
        this.mapper = mapper;
        this.context = context;
    }
    public async Task Create(CreatePositionInputModel inputModel)
    {
        Position position = this.mapper.Map<Position>(inputModel);

        await context.Positions.AddAsync(position);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<PositionsAllViewModel>> GetAll()
        => await this.context
            .Positions
            .ProjectTo<PositionsAllViewModel>(this.mapper.ConfigurationProvider)
            .ToArrayAsync();
}