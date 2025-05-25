using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using WebShop.CatalogAPI.Data;
using WebShop.CatalogAPI.Entities;
namespace WebShop.CatalogAPI;

public static class CatalogApi
{
    public static void MapItemEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Item").WithTags(nameof(CatalogItem));

        group.MapGet("/", async (CatalogDbContext db) =>
        {
            return await db.Item.ToListAsync();
        })
        .WithName("GetAllItems")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<CatalogItem>, NotFound>> (int itemid, CatalogDbContext db) =>
        {
            return await db.Item.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == itemid)
                is CatalogItem model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetItemById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int itemid, CatalogItem item, CatalogDbContext db) =>
        {
            var affected = await db.Item
                .Where(model => model.Id == itemid)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, item.Id)
                    .SetProperty(m => m.Name, item.Name)
                    .SetProperty(m => m.AvailableStock, item.AvailableStock)
                    .SetProperty(m => m.PictureFileName, item.PictureFileName)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateItem")
        .WithOpenApi();

        group.MapPost("/", async (CatalogItem item, CatalogDbContext db) =>
        {
            db.Item.Add(item);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Item/{item.Id}",item);
        })
        .WithName("CreateItem")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int itemid, CatalogDbContext db) =>
        {
            var affected = await db.Item
                .Where(model => model.Id == itemid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteItem")
        .WithOpenApi();
    }
}
