using AutoFixture;

namespace DrWhoConsoleApp.UnitTests.Customisations
{
    public class DateOnlyCustomisation : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var year = new Random().Next(1970, DateTime.Now.Year);
            var month = new Random().Next(1, 12);
            var day = new Random().Next(1, 28);
            fixture.Register(() => new DateOnly(year, month, day));
        }
    }
}