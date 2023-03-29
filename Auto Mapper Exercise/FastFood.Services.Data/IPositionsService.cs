using FastFood.Web.ViewModels.Positions;

namespace FastFood.Services.Data;

public interface IPositionsService
{
    Task Create(CreatePositionInputModel inputModel);

    Task<IEnumerable<PositionsAllViewModel>> GetAll();
}