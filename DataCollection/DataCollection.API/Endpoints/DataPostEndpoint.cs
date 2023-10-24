using DataCollection.API.DTOs;
using DataCollection.Application;
using FastEndpoints;

namespace DataCollection.API.Endpoints;

public class DataPostEndpoint : Endpoint<DataPostDto>
{
    public DataPostEndpoint(ILogger<DataPostEndpoint> logger, IDataProcessor dataProcessor)
    {
        _logger = logger;
        _dataProcessor = dataProcessor;
    }

    private readonly ILogger<DataPostEndpoint> _logger;
    private readonly IDataProcessor _dataProcessor;

    public override void Configure()
    {
        Post("/api/data");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DataPostDto req, CancellationToken ct)
    {
        _logger.LogInformation("Received event \"{event_name}\".", req.EventName);
        await _dataProcessor.ProcessEventAsync(req.EventName);
        await SendOkAsync(ct);
    }
}