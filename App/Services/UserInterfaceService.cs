using DrWhoConsoleApp.Interfaces;
using DrWhoConsoleApp.Models;
using Microsoft.Extensions.Logging;

namespace DrWhoConsoleApp.Services
{
    public class UserInterfaceService : IUserInterfaceService
    {
        private readonly IDoctorService _doctorService;
        private readonly IEpisodeService _episodeService;
        private readonly IConsoleService _consoleService;
        private readonly ILogger<IUserInterfaceService> _logger;

        public UserInterfaceService(
            IDoctorService doctorService,
            IEpisodeService episodeService,
            IConsoleService consoleService,
            ILogger<IUserInterfaceService> logger
            )
        {
            _doctorService = doctorService;
            _episodeService = episodeService;
            _consoleService = consoleService;
            _logger = logger;
        }

        public void Run()
        {
            try
            {
                while (true)
                {
                    _consoleService.WriteLine("1. List doctors");
                    _consoleService.WriteLine("2. List episodes by Year");
                    _consoleService.WriteLine("3. Add a new doctor");
                    _consoleService.WriteLine("4. Exit");
                    _consoleService.Write("Enter your choice: ");
                    var choice = _consoleService.ReadLine();

                    _consoleService.WriteLine("==============================================================================================================");
                    _consoleService.WriteLine(string.Empty);

                    switch (choice)
                    {
                        case "1":
                            ListDoctors();
                            break;
                        case "2":
                            ListEpisodesByYear();
                            break;
                        case "3":
                            AddNewDoctor();
                            break;
                        case "4":
                            return;
                        default:
                            _consoleService.WriteLine("Invalid choice. Please try again.");
                            _logger.Log(LogLevel.Warning, $"Invalid choice ({choice}) entered by the user");
                            break;
                    }

                    _consoleService.WriteLine("==============================================================================================================");
                    _consoleService.WriteLine(string.Empty);
                }
            }
            catch (Exception ex)
            {
                var errorText = $"An error occurred: {ex.Message}";
                _logger.Log(LogLevel.Error, errorText);
                _consoleService.WriteLine(errorText);
            }
        }

        private void ListDoctors()
        {
            var doctors = _doctorService.GetAllDoctors();
            _consoleService.WriteLine($"There are {doctors.Count()} doctors in the database");
            int i = 0;
            foreach (var doctor in doctors)
            {
                i++;
                _consoleService.WriteLine($"{i}. Doctor Name: {doctor.DoctorName}, born: {doctor.BirthDate}");
            }
        }

        private void ListEpisodesByYear()
        {
            bool v = false;
            int yearInt = 0;
            int.TryParse("0", out yearInt);

            while (true)
            {
                _consoleService.Write($"Please enter the year you want to list the episodes for: ");
                var year = _consoleService.ReadLine();
                v = int.TryParse(year, out yearInt);

                if (v)
                {
                    break;
                }
                else
                {
                    _consoleService.WriteLine("Invalid year. Please try again.");
                    _logger.Log(LogLevel.Warning, $"Invalid year ({year}) entered by the user");
                }
            }

            var allEpisodes = _episodeService.GetAllEpisodes();
            var episodes = allEpisodes
                                .Where(x => x.EpisodeDate.HasValue && x.EpisodeDate.Value.Year == yearInt)
                                .ToList();

            _consoleService.WriteLine($"There is a total of {allEpisodes.Count()} episodes in the database");
            _consoleService.WriteLine($"There are {episodes.Count()} episodes in the database broadcasted in {yearInt}");
            int i = 0;
            foreach (var episode in episodes)
            {
                i++;
                _consoleService.WriteLine($"{i}. Episode: {episode.Title}, Date: {episode.EpisodeDate}, Doctor: {episode.Doctor.DoctorName}");
            }
        }

        private void AddNewDoctor()
        {
            _consoleService.Write("Enter the new doctor's name:");
            var doctorName = _consoleService.ReadLine();
            _consoleService.Write("Enter the new doctor's birthdate (yyyy-mm-dd):");
            var birthdate = _consoleService.ReadLine();

            try
            {
                var newDoctor = new Doctor { DoctorName = doctorName, BirthDate = DateOnly.Parse(birthdate) };
                _doctorService.AddDoctor(newDoctor);
                _consoleService.WriteLine($"Doctor {newDoctor.DoctorName} with id {newDoctor.DoctorId} added");
            }
            catch (Exception ex)
            {
                var errorText = $"An error occurred while adding a new doctor with doctorName {doctorName} and birthdate {birthdate}" +
                    $"\nError: {ex.Message}";
                _logger.Log(LogLevel.Error, errorText);
                _consoleService.WriteLine(errorText);
            }
        }
    }
}