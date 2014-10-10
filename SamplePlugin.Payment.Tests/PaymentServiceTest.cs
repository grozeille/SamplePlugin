using Autofac.Extras.CommonServiceLocator;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using NFluent;
using SamplePlugin.Catalog.Api;
using SamplePlugin.Catalog.Api.Model;
using SamplePlugin.Catalog.Api.Services;
using System;
using Xunit;

namespace SamplePlugin.Payment.Tests
{
    public class PaymentServiceTest
    {
        [Fact]
        public void It_call_the_service()
        {
            using (var fake = new AutoFake())
            {
                var csl = new AutofacServiceLocator(fake.Container);
                ServiceLocator.SetLocatorProvider(() => csl);

                // Given
                var fakeProduct = new Product { Id = 0, Name = "Fake!", Description = "", Price = 99.9 };
                A.CallTo(() => fake.Resolve<ICatalogRepository>().FindAllProducts()).Returns(new Product[] { fakeProduct });
                A.CallTo(() => fake.Resolve<ICatalogService>().TryBuyProduct(A<BuyRequest>.Ignored)).Returns(new BuyResponse { OK = true, ConfirmationGuid = Guid.Empty });
                var service = fake.Resolve<PaymentService>();

                // When
                var result = service.Body;

                // Then
                A.CallTo(() => fake.Resolve<ICatalogService>().TryBuyProduct(A<BuyRequest>.Ignored)).MustHaveHappened();
                var expected = "<div class='panel-body'>Achat du produit Fake!: OK (numéro de commande: #" + Guid.Empty + ")</div>";
                Check.That(result).IsEqualTo(expected);
            }
        }
    }
}
