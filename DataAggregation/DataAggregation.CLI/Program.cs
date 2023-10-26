using DataAggregation.CLI;
using DataAggregation.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddInfrastructure();

builder.Services.AddLogging();

builder.Services.AddApplication();

builder.Services.AddEventsConsumer();
builder.Services.AddEventsStorage();

builder.Services.AddDbContext(builder.Configuration);

builder.Services.AddHostedService<EventMessagesConsumeWorker>();

using IHost host = builder.Build();

host.ApplyMigrations();

host.Run();