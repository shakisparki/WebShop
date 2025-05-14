var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.WebShop_ApiService>("apiservice");

builder.AddProject<Projects.WebShop_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(apiService);

builder.AddProject<Projects.WebShop_OrderingAPI>("webshop-orderingapi");

builder.Build().Run();
