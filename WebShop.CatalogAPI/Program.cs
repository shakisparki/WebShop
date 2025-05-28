using WebShop.CatalogAPI.Data;
using WebShop.CatalogAPI;
using WebShop.CatalogAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddNpgsqlDbContext<CatalogDbContext>("catalogdb");

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();

app.MapItemEndpoints();

app.SetupDatabase();    

app.Run();