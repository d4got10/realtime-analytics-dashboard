using Authorization.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext(configuration);
builder.Services.AddApplication(configuration);
builder.Services.AddFastEndpoints().SwaggerDocument();

WebApplication app = builder.Build();

app.UseFastEndpoints().UseSwaggerGen();

app.Run();