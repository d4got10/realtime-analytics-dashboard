using DataAggregation.API.Dtos;
using DataAggregation.Application;
using DataAggregation.Domain;
using FastEndpoints;

namespace DataAggregation.API.Endpoints;

public class EventListEndpoint : Endpoint<EventsPaginationDto, IEnumerable<EventDto>>
{
    public EventListEndpoint(IEventsRepository repository, ILogger<EventListEndpoint> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    private readonly IEventsRepository _repository;
    private readonly ILogger<EventListEndpoint> _logger;

    public override void Configure()
    {
        AllowAnonymous();
        Get("/api/events");
        
        Description(b => b
            .Produces<IEnumerable<EventDto>>(StatusCodes.Status200OK, "application/json"));
        
        Summary(s =>
        {
            s.Summary = "Retrieve events";
            s.Description = "Retrieves page of events";
            s.RequestParam(x => x.PageIndex, "Index of the page");
            s.RequestParam(x => x.PerPageCount, "Number of items per page");
        });
    }

    public override async Task HandleAsync(EventsPaginationDto req, CancellationToken ct)
    {
        IEnumerable<Event> page = await _repository.GetPageNoTrackingAsync(req.PageIndex, req.PerPageCount);
        await SendAsync(page.Select(x => new EventDto
        {
            Id = x.Id,
            Name = x.Name
        }), cancellation: ct);
    }
}