using EventMi.Core.Contracts;
using EventMi.Core.Models;
using EventMi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventMi.Controllers;

public class EventController : Controller
{
    private readonly ILogger _logger;
    private readonly IEventService _service;
    
    public EventController(ILogger<EventController> logger, IEventService service)
    {
        this._logger = logger;
        this._service = service;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<EventModel> events;
        
        try
        {
            events = await this._service.GetAllEventsAsync();
        }
        catch (Exception e)
        {
            this._logger.LogError("EventController/Index", e);
            return View("Error", new ErrorViewModel()
            {
                Message = "There was an unexpected error"
            });

        }
        return View("All", events);
    }
}