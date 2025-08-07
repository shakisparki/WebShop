var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var basketcache = builder.AddRedis("basketcache");
var rabbitMq = builder.AddRabbitMQ("rabbitmq").WithManagementPlugin(); 
var postgres = builder.AddPostgres("postgres");

var catalogdb = postgres.AddDatabase("catalogdb");

var catalogAPI = builder.AddProject<Projects.WebShop_CatalogAPI>("catalogapi")
                .WithReference(catalogdb)
                .WithReference(rabbitMq);

var orderingAPI = builder.AddProject<Projects.WebShop_OrderingAPI>("orderingapi");

var basketAPI = builder.AddProject<Projects.WebShop_BasketAPI>("basketapi")
    .WithReference(basketcache)
    .WithReference(rabbitMq);

builder.AddProject<Projects.WebShop_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(basketAPI)
    .WithReference(catalogAPI);

builder.Build().Run();