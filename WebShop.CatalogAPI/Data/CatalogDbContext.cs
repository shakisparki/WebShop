using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebShop.CatalogAPI.Entities;

namespace WebShop.CatalogAPI.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext (DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
        }

        public DbSet<WebShop.CatalogAPI.Entities.CatalogItem> Items { get; set; } = default!;
    }
}
