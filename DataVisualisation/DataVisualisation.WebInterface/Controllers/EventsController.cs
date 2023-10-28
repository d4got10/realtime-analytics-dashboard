using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DataVisualisation.WebInterface.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController : ControllerBase
{
    public EventsController(IHttpClientFactory httpClientFactory, ILogger<EventsController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<EventsController> _logger;

    private const string ApiUrl = "http://data_aggregation:5000/api/events?page_index=0&per_page_count=100";
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        using HttpClient httpClient = _httpClientFactory.CreateClient();
        
        string response = await httpClient.GetStringAsync(ApiUrl);
        Event[]? events = JsonConvert.DeserializeObject<Event[]>(response);
        if (events == null)
            return Problem();

        return Ok(events);
    }
}