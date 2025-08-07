using WebShop.CatalogAPI.Data;
using WebShop.CatalogAPI;
using WebShop.CatalogAPI.Extensions;
using WebShop.EventBus;

var builder = WebApplication.CreateBuilder(args);
builder.AddNpgsqlDbContext<CatalogDbContext>("catalogdb");

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped(
    serviceProvider => new RabbitMqService(
        builder.Configuration.GetConnectionString("rabbitmq") ?? ""
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapDefaultEndpoints();

app.MapItemEndpoints();

app.SetupDatabase();    

app.Run();