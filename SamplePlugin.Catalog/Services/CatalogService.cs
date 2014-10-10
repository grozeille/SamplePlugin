using bbv.Common.EventBroker;
using bbv.Common.EventBroker.Handlers;
using bbv.Common.Events;
using SamplePlugin.Api;
using SamplePlugin.Catalog.Api;
using SamplePlugin.Catalog.Api.Model;
using SamplePlugin.Catalog.Api.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SamplePlugin.Catalog.Services
{
    public class CatalogService : IService, ICatalogService
    {
        private readonly ICatalogRepository _repository;

        private readonly IDictionary<int, int> _stock = new Dictionary<int, int>();

        public CatalogService(ICatalogRepository repository, IEventBroker eventBroker)
        {
            _repository = repository;
            eventBroker.Register(this);

            // set the stock to 10
            foreach (var product in _repository.FindAllProducts())
            {
                _stock[product.Id] = 10;
            }
        }

        public String Title
        {
            get { return "Catalogue"; }
        }

        public String Body
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("<table class='table'><tr><th>Produit</th><th>Description</th><th>Prix</th><th>Stock</th></tr>");
                foreach (var product in _repository.FindAllProducts())
                {
                    builder
                        .AppendFormat("<tr><td>{0}</td><td>{1}</td><td>{2:0.00}</td><td>{3}</td></tr>", product.Name, product.Description, product.Price, _stock[product.Id]).AppendLine();
                    //.AppendLine("<div class='row'><div class='col-md-12'>")
                    //.AppendFormat("<h4>{0}</h4><br/><b>Description:</b> {1}<br/><b>Prix:</b> {2:0.00} €<br/><b>Stock:</b> {3}", product.Name, product.Description, product.Price, _stock[product.Id]).AppendLine()
                    //.AppendLine("</div></div>");
                }
                builder.AppendLine("</table>");
                return builder.ToString();
            }
        }

        public BuyResponse TryBuyProduct(BuyRequest request)
        {
            var response = new BuyResponse
            {
                ProductId = request.ProductId,
                ConfirmationGuid = Guid.NewGuid()
            };
            if (_stock[request.ProductId] > 0)
            {
                response.OK = true;
            }
            else
            {
                response.OK = false;
            }

            return response;
        }

        [EventSubscription(CatalogTopics.ON_BUY_PRODUCT, typeof(Background))]

        public void OnBuyResponse(object sender, EventArgs<BuyResponse> e)
        {
            // decrease the stock
            _stock[e.Value.ProductId] = _stock[e.Value.ProductId] - 1;
        }
    }
}
