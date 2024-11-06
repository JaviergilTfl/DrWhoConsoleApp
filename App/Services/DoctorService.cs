using DrWhoConsoleApp.DatabaseContext;
using DrWhoConsoleApp.Interfaces;
using DrWhoConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DrWhoConsoleApp.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly DoctorWhoContext _context;
        private readonly ILogger<DoctorService> _logger;

        public DoctorService(DoctorWhoContext context, ILogger<DoctorService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            return _context
                .Doctors
                .Include(d => d.Episodes)
                .ToList();
        }

        public async Task<int> AddDoctor(Doctor doctor)
        {
            try
            {
                await _context.Doctors.AddAsync(doctor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException due)
            {
                _logger.LogError($"DoctorService - {nameof(AddDoctor)} - Sql query execution failed. Exception - {due.Message}");
                throw;
            }

            return doctor.DoctorId;
        }
    }
}