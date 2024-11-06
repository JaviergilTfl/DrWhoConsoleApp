using DrWhoConsoleApp.Models;

namespace DrWhoConsoleApp.Interfaces
{
    public interface IDoctorService
    {
        IEnumerable<Doctor> GetAllDoctors();
        Task<int> AddDoctor(Doctor doctor);
    }
}
