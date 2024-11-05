using DrWhoConsoleApp.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DrWhoConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Application...");
            await RunAsync();
        }

        public static async Task RunAsync()
        {
            try
            {
                var host = StartUp.CreateHost().Build();

                var serviceRunner = host.Services.GetService<IServiceRunner>();

                Console.WriteLine("Starting application runner...");

                if (serviceRunner != null) await serviceRunner.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}