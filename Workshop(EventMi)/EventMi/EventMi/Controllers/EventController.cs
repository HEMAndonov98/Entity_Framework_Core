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
    /// <param name="logger">ILogger of type EventController</param>
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

    /// <summary>
    /// This method adds the data provided in the Model to the database
    /// </summary>
    /// <param name="model">represents Dto for adding Event to Db</param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(EventModel model)
    {
        try
        {
            await this._service.AddEventAsync(model);
        }
        catch (ArgumentNullException nullException)
        {
            this._logger.LogError("[HttpPost]/EventController/Add", nullException);
            return View("Error", new ErrorViewModel()
            {
                Message = "There was an error while trying to add an Event"
            });
        }
        catch (Exception e)
        {
            this._logger.LogError("[HttpPost]/EventController/Add", e);
            return View("Error", new ErrorViewModel()
            {
                Message = "There was an unexpected error"
            });
        }
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Method to show the client all details about a given entity
    /// </summary>
    /// <param name="id">event identifier</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        EventModel model;
        try
        {
            model = await this._service.GetEvent(id);
        }
        catch (Exception e)
        {
            this._logger.LogError("[HttpGet]/EventController/Details", e);
            return View("Error", new ErrorViewModel()
            {
                Message = "There was an unexpected error"
            });
        }
        return View("Details", model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        EventModel model;
        try
        { 
            model = await this._service.GetEvent(id);
        }
        catch (Exception e)
        {
            this._logger.LogError("[HttpGet]/EventController/Edit", e);
            return View("Error", new ErrorViewModel()
            {
                Message = "There was an unexpected error"
            });
        }
        return View("Edit", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EventModel model)
    {
        try
        {
            await this._service.UpdateEventAsync(model);
        }
        catch (Exception e)
        {
            this._logger.LogError("[HttpPut]/EventController/Edit", e);
            return View("Error", new ErrorViewModel()
            {
                Message = "A problem occured while trying to update the Event"
            });
        }

        return View("Details", model);
    }
}