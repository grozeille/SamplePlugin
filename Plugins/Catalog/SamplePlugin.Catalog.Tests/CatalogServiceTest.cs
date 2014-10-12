using Autofac.Extras.CommonServiceLocator;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using NFluent;
using SamplePlugin.Catalog.Api.Model;
using SamplePlugin.Catalog.Api.Services;
using SamplePlugin.Catalog.Services;
using Xunit;

namespace SamplePlugin.Catalog.Tests
{
    public class CatalogServiceTest
    {
        [Fact]
        public void It_call_the_repository()
        {
            using (var fake = new AutoFake())
            {
                var csl = new AutofacServiceLocator(fake.Container);
                ServiceLocator.SetLocatorProvider(() => csl);

                // Given
                var fakeProduct = new Product { Id = 0, Name = "Fake!", Description = "", Price = 99.9 };
                A.CallTo(() => fake.Resolve<ICatalogRepository>().FindAllProducts()).Returns(new Product[] { fakeProduct });
                var service = fake.Resolve<CatalogService>();

                // When
                var result = service.Body;

                // Then
                A.CallTo(() => fake.Resolve<ICatalogRepository>().FindAllProducts()).MustHaveHappened();
                var expected =
                    "<table class='table'><tr><th>Produit</th><th>Description</th><th>Prix</th><th>Stock</th></tr>\r\n" +
                    "<tr><td>Fake!</td><td></td><td>99,90</td><td>10</td></tr>\r\n" +
                    "</table>\r\n";

                Check.That(result).IsEqualTo(expected);
            }
        }
    }
}
