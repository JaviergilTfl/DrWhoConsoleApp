using DrWhoConsoleApp.Interfaces;
using System.Diagnostics;

namespace DrWhoConsoleApp.Services
{
    public class ServiceRunner : IServiceRunner
    {
        private readonly IUserInterfaceService _userInterfaceService;

        public ServiceRunner(IUserInterfaceService userInterfaceService)
        {
            _userInterfaceService = userInterfaceService;
        }

        public async Task Run()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            _userInterfaceService.Run();

            // Diagnostics
            stopwatch.Stop();
            var elapsed_time = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Elapse time in milliseconds: {elapsed_time}");
        }
    }
}