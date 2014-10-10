using bbv.Common.EventBroker;
using bbv.Common.Events;
using Microsoft.Practices.ServiceLocation;
using SamplePlugin.Api;
using SamplePlugin.Catalog.Api;
using SamplePlugin.Catalog.Api.Model;
using SamplePlugin.Catalog.Api.Services;
using System;
using System.Linq;

namespace SamplePlugin.Payment
{
    public class PaymentService : IService
    {
        private readonly ICatalogRepository _catalogRepository;
        private readonly ICatalogService _catalogService;

        /// <summary>
        /// Publish the "OnBuyProduct" event
        /// </summary>
        [EventPublication(CatalogTopics.ON_BUY_PRODUCT)]
        public event EventHandler<EventArgs<BuyResponse>> OnBuyProduct;

        public PaymentService(ICatalogRepository catalogRepository, IEventBroker eventBroker)
        {
            // using Autofac injection
            _catalogRepository = catalogRepository;
            eventBroker.Register(this);

            // using Service Locator
            _catalogService = ServiceLocator.Current.GetInstance<ICatalogService>();
        }

        public String Title
        {
            get { return "Achats"; }
        }

        public String Body
        {
            get
            {
                // call other services
                var firstProduct = _catalogRepository.FindAllProducts().First();
                var result = _catalogService.TryBuyProduct(new BuyRequest { ProductId = firstProduct.Id });

                if (result.OK)
                {
                    // notify using broker
                    if (OnBuyProduct != null)
                    {
                        OnBuyProduct(this, new EventArgs<BuyResponse>(result));
                    }
                }

                return String.Format("<div class='panel-body'>Achat du produit {0}: {1} (numéro de commande: #{2})</div>", firstProduct.Name, result.OK ? "OK" : "Erreur", result.ConfirmationGuid);
            }
        }
    }
}
