using WebShop.BasketAPI.Repositories;
using WebShop.BasketAPI.Services;
using WebShop.EventBus;
using WebShop.EventBus.Events;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRedisClient("basketcache"); //, opt => opt.ConnectionString = builder.Configuration.GetConnectionString("basketcache")
builder.Services.AddTransient<IBasketRepository, BasketRepository>(); //Addscoped??
builder.Services.AddSingleton(
    serviceProvider => new RabbitMqService(
        builder.Configuration.GetConnectionString("rabbitmq") ?? ""
    )
);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseDeveloperExceptionPage(); //add condition to this later

// Configure the HTTP request pipeline.
app.MapGrpcService<BasketService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

var rmqs = app.Services.GetService<RabbitMqService>();
rmqs.SetupEventBus(); //shouldnt be done here but good to test
rmqs.AddSubscription(EventQueues.BasketQueue,(msg) =>
{
    if (msg.Type == EventTypes.PriceChanged)
    {
        var ev = msg as PriceChangedEvent;
        Console.WriteLine($"Price Changed from {ev?.OldPrice} to {ev?.NewPrice}");
    }
    
});

app.Run();

