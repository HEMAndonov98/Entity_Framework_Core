using EventMi.Core.Contracts;
using EventMi.Core.Models;
using EventMi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventMi.Controllers;
/// <summary>
/// This is an MVC controller for an Event
/// </summary>
public class EventController : Controller
{
    /// <summary>
    /// Standard logger
    /// </summary>
    private readonly ILogger _logger;
    /// <summary>
    /// this is an interface of the services this controller requires to work correctly
    /// </summary>
    private readonly IEventService _service;
    
    /// <summary>
    /// Constructor for the controller determines dependencies 
    /// </summary>
    /// <param name="logger">ILogger<EventController></param>
    /// <param name="service">IEventService</param>
    public EventController(ILogger<EventController> logger, IEventService service)
    {
        this._logger = logger;
        this._service = service;
    }

    /// <summary>
    /// This method returns and visualizes a list of events to the browser
    /// </summary>
    /// <returns>IActionResult/View</returns>
    [HttpGet]
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

    /// <summary>
    /// This method visualizes the form for adding an event to the database
    /// </summary>
    /// <returns>IActionResult</returns>
    [HttpGet]
    public IActionResult Add()
    {
        return View("Add");
    }

    [HttpPost]
    public async Task<IActionResult> Add(EventModel model)
    {
        await this._service.AddEventAsync(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        EventModel model = await this._service.GetEvent(id);
        return View("Details", model);
    }
}