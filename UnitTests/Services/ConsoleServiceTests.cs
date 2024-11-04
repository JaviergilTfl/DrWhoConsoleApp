using DrWhoConsoleApp.Services;
using DrWhoConsoleApp.UnitTests;

namespace UnitTests.Services
{
    // To write unit tests for ConsoleService.cs, you need to mock the Console class methods.
    // However, since Console methods are static, you can't directly mock them.
    // Instead, you can use a library like System.IO.Abstractions to abstract the console operations,
    // or you can use TextWriter and TextReader to redirect the console output and input.
    public class ConsoleServiceTests
    {
        [Theory, AutoDomainData]
        public void WriteLine_ShouldWriteMessageToConsole(
            ConsoleService consoleService,
            TextWriter stringWriter,
            string message)
        {
            // Act
            Console.SetOut(stringWriter);
            consoleService.WriteLine(message);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Is.EqualTo(message + Environment.NewLine));
        }

        [Theory, AutoDomainData]
        public void Write_ShouldWriteMessageToConsole(
            ConsoleService consoleService,
            TextWriter stringWriter,
            string message)
        {
            // Act
            Console.SetOut(stringWriter);
            consoleService.Write(message);

            // Assert
            var output = stringWriter.ToString();
            Assert.That(output, Is.EqualTo(message));
        }

        [Theory, AutoDomainData]
        public void ReadLine_ShouldReadMessageFromConsole(
            ConsoleService consoleService,
            TextReader stringReader,
            string input)
        {
            // Arrange
            var stringReaderWithInput = new StringReader(input);
            Console.SetIn(stringReaderWithInput);

            // Act
            var result = consoleService.ReadLine();

            // Assert
            Assert.That(result, Is.EqualTo(input));
        }
    }
}