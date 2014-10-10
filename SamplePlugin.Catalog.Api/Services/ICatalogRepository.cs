using SamplePlugin.Catalog.Api.Model;
using System.Collections.Generic;

namespace SamplePlugin.Catalog.Api.Services
{
    public interface ICatalogRepository
    {
        IEnumerable<Product> FindAllProducts();
    }
}
