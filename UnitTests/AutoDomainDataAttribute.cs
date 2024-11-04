using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Disruptions.UnitTests.Common.Customisations;
using DrWhoConsoleApp.UnitTests.Customisations;
using System.Diagnostics.CodeAnalysis;

namespace DrWhoConsoleApp.UnitTests
{
    [ExcludeFromCodeCoverage]
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute() : base(GetDefaultFixture)
        {
        }

        public static IFixture GetDefaultFixture()
        {
            AutoMoqCustomization autoMoqCustomization = new AutoMoqCustomization
            {
                ConfigureMembers = true
            };

            return new Fixture()
                .Customize(new DomainCustomisation())
                .Customize(new DateOnlyCustomisation())
                .Customize(new ConsoleCustomization())
                .Customize(new DrWhoInMemoryCustomization())
                .Customize(autoMoqCustomization);
        }
    }

    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] objects) : base(AutoDomainDataAttribute.GetDefaultFixture, objects) { }
    }
}