var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var rabbitMq = builder.AddRabbitMQ("rabbitmq"); 
   
var apiService = builder.AddProject<Projects.WebShop_ApiService>("apiservice");
var orderingAPI = builder.AddProject<Projects.WebShop_OrderingAPI>("orderingapi");

builder.AddProject<Projects.WebShop_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.Build().Run();