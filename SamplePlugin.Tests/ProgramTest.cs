using Autofac.Extras.CommonServiceLocator;
using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using Microsoft.Practices.ServiceLocation;
using SamplePlugin.Api;
using SamplePlugin.App;
using System;
using Xunit;

namespace SamplePlugin.Tests
{
    public class ProgramTest
    {
        [Fact]
        public void It_call_the_service()
        {
            using (var fake = new AutoFake())
            {
                // arrange
                var csl = new AutofacServiceLocator(fake.Container);
                ServiceLocator.SetLocatorProvider(() => csl);

                A.CallTo(() => fake.Resolve<IMyService>().SayHello()).Invokes(() => Console.WriteLine("Fake!"));
                var program = fake.Resolve<Program>();

                // act
                program.Run();

                // assert
                A.CallTo(() => fake.Resolve<IMyService>().SayHello()).MustHaveHappened();                
            }
        }
    }
}
