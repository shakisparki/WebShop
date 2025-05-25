var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");
var rabbitMq = builder.AddRabbitMQ("rabbitmq"); 
var postgres = builder.AddPostgres("postgres");

var catalogdb = postgres.AddDatabase("catalogdb");

var catalogAPI = builder.AddProject<Projects.WebShop_CatalogAPI>("catalogapi")
                .WithReference(catalogdb);

var orderingAPI = builder.AddProject<Projects.WebShop_OrderingAPI>("orderingapi");

builder.AddProject<Projects.WebShop_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WithReference(catalogAPI);

builder.Build().Run();