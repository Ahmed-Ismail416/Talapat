using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatRepository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext _dbcontext)
        {
            if(!_dbcontext.productBrands.Any())
            {
                // Reading the data from the json files
                var BrandData = File.ReadAllText("../TalabatRepository/Data/DataSeed/brands.json");
                // Deserializing the data
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if (Brands?.Count() > 0)
                {

                    foreach (var item in Brands)
                    {
                        await _dbcontext.Set<ProductBrand>().AddAsync(item);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }

            if(!_dbcontext.ProductTypes.Any())
            {
                //seeding ProductTypes
                var ProductTypeData = File.ReadAllText("../TalabatRepository/Data/DataSeed/types.json");
                var ProductTypes = JsonSerializer.Deserialize<List<ProductType>>(ProductTypeData);
                if (ProductTypes?.Count() > 0)
                {
                    foreach (var item in ProductTypes)
                    {
                        await _dbcontext.Set<ProductType>().AddAsync(item);
                    }
                    await _dbcontext.SaveChangesAsync();
                }
            }
            //seeding product
            if (!_dbcontext.Products.Any())
            {
                var ProductData = File.ReadAllText("../TalabatRepository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                if (Products?.Count() > 0)
                {
                    foreach (var item in Products)
                    {
                        await _dbcontext.Set<Product>().AddAsync(item);
                    }
                }
                await _dbcontext.SaveChangesAsync();
            }
        }
    }
}
