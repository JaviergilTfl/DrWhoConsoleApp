using AutoFixture;

namespace DrWhoConsoleApp.UnitTests.Customisations
{
    public class DomainCustomisation : ICustomization
    {
        public static int RepeatCount { get; private set; }

        public void Customize(IFixture fixture)
        {
            RepeatCount = 3;
            fixture.RepeatCount = RepeatCount;
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }
    }
}