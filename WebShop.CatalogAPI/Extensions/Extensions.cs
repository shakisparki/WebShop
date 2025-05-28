using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebShop.CatalogAPI.Data;
using WebShop.CatalogAPI.Entities;

namespace WebShop.CatalogAPI.Extensions
{
    public static class Extensions
    {
        public static void SetupDatabase(this WebApplication app)
        {
            //migrate db
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
            db.Database.Migrate();
            db.SaveChanges();

            SeedDatabase(db);
        }

        private static void SeedDatabase(CatalogDbContext db)
        {
            var items = File.ReadAllText("catalog.json");
            var entries = JsonSerializer.Deserialize<List<CatalogSourceEntry>>(items);
            if (entries == null || !entries.Any())
            {
                throw new InvalidOperationException("No catalog items found in the JSON file.");
            }
            var catalogitems = entries.Select(x => new CatalogItem
            {
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                PictureFileName = $"{x.Id}.webp",
                AvailableStock = x.Id,
                RestockThreshold = (x.Id % 5) + 1,
                MaxStockThreshold = (x.Id % 8) + 2
            });

            db.Items.AddRange(catalogitems);
            db.SaveChanges();
        }

        private class CatalogSourceEntry
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public string Brand { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }
    }
}
