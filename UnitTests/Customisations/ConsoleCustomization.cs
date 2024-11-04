using AutoFixture;

namespace DrWhoConsoleApp.UnitTests.Customisations
{
    public class ConsoleCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var stringWriter = new StringWriter();
            var stringReader = new StringReader(string.Empty);

            fixture.Register(() => stringWriter);
            fixture.Register(() => stringReader);

            fixture.Register<TextWriter>(() => stringWriter);
            fixture.Register<TextReader>(() => stringReader);

            Console.SetOut(stringWriter);
            Console.SetIn(stringReader);
        }
    }

    //public class ConsoleCustomization : ICustomization
    //{
    //    public void Customize(IFixture fixture)
    //    {
    //        var stringWriter = new StringWriter();
    //        var stringReader = new StringReader(string.Empty);

    //        fixture.Register(() => stringWriter);
    //        fixture.Register(() => stringReader);

    //        fixture.Register<TextWriter>(() => stringWriter);
    //        fixture.Register<TextReader>(() => stringReader);

    //        Console.SetOut(stringWriter);
    //        Console.SetIn(stringReader);
    //    }
    //}

    //public class AutoDomainDataAttribute : AutoDataAttribute
    //{
    //    public AutoDomainDataAttribute()
    //        : base(() => new Fixture()
    //            .Customize(new AutoMoqCustomization())
    //            .Customize(new ConsoleCustomization()))
    //    {
    //    }
    //}
}
