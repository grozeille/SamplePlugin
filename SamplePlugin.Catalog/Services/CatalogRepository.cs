
using SamplePlugin.Catalog.Api.Model;
using SamplePlugin.Catalog.Api.Services;
using System;
using System.Collections.Generic;
namespace SamplePlugin.Catalog.Services
{
    public class CatalogRepository : ICatalogRepository
    {
        public IEnumerable<Product> FindAllProducts()
        {
            var random = new Random();
            for (int cpt = 0; cpt < 5; cpt++)
            {
                yield return new Product
                {
                    Id = cpt,
                    Name = "Product#"+cpt,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Price = random.NextDouble()*100.0
                };  
            }
        }
    }
}
