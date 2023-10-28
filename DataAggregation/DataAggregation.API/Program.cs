using DataAggregation.API.Workers;
using DataAggregation.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddInfrastructure();

builder.Services.AddLogging();

builder.Services.AddApplication();

builder.Services.AddEventsConsumer();
builder.Services.AddEventsStorage();

builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddHostedService<EventMessagesConsumeWorker>();

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();

WebApplication app = builder.Build();

app.UseFastEndpoints()
    .UseSwaggerGen();

//TODO: remove
app.ApplyMigrations();

app.Run();