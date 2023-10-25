using DataAggregation.CLI;
using DataAggregation.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddLogging();

builder.Services.AddApplication();
builder.Services.AddKafkaEventsConsumer();

builder.Services.AddHostedService<EventMessagesConsumeWorker>();

using IHost host = builder.Build();

host.Run();