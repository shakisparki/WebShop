using WebShop.BasketAPI.Repositories;
using WebShop.BasketAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisClient("basketcache"); //, opt => opt.ConnectionString = builder.Configuration.GetConnectionString("basketcache")
builder.Services.AddTransient<IBasketRepository, BasketRepository>(); //Addscoped??

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseDeveloperExceptionPage(); //add condition to this later

// Configure the HTTP request pipeline.
app.MapGrpcService<BasketService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

