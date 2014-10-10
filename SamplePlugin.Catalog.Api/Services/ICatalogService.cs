using SamplePlugin.Catalog.Api.Model;

namespace SamplePlugin.Catalog.Api
{
    public interface ICatalogService
    {
        BuyResponse TryBuyProduct(BuyRequest request);
    }
}
