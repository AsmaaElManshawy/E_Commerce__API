using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.DataSeeding
{
    public class CatalogDataSeed(StoreDbContext dbContext) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingmigrations = await dbContext.Database.GetAppliedMigrationsAsync();
                if (pendingmigrations.Any())
                    await dbContext.Database.MigrateAsync(ct);

                var rootPath = Path.Combine(AppContext.BaseDirectory, "DataSeed");

                await SeedDataIfEmptyAsync<ProductBrand, int>(rootPath, "brands.json", ct);
                await SeedDataIfEmptyAsync<ProductType, int>(rootPath, "types.json", ct);
                await SeedDataIfEmptyAsync<Product, int>(rootPath, "products.json", ct);

                var result = await dbContext.SaveChangesAsync(ct);
                if (result > 0)
                    Console.WriteLine($"Data seeded successfully. {result} rows affected");
                else
                    Console.WriteLine("Failed to Seed Data.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // helper method to read data from a JSON file

        private async Task SeedDataIfEmptyAsync<T, TKey>(string rootPath, string fileName, CancellationToken ct) where T : BaseEntity<TKey>
        {
            if (await dbContext.Set<T>().AnyAsync())
                return;

            var filePath = Path.Combine(rootPath, fileName);

            if (!File.Exists(filePath))
                return;

            using var fileStream = File.OpenRead(filePath);

            var items = await JsonSerializer.DeserializeAsync<List<T>>(fileStream, cancellationToken: ct);

            if (items?.Any() ?? false)
                dbContext.Set<T>().AddRange(items);

        }
    }
}
