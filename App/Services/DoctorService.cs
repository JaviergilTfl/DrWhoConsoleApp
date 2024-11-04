using DrWhoConsoleApp.DatabaseContext;
using DrWhoConsoleApp.Interfaces;
using DrWhoConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DrWhoConsoleApp.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorWhoContext _context;

        public DoctorService(IDoctorWhoContext context)
        {
            _context = context;
        }

        public IEnumerable<Doctor> GetAllDoctors()
        {
            return _context
                .Doctors
                .Include(d => d.Episodes)
                .ToList();
        }

        public void AddDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();
        }
    }
}