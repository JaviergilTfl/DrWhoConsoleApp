using DrWhoConsoleApp.Models;

namespace DrWhoConsoleApp.Interfaces
{
    public interface IDoctorService
    {
        IEnumerable<Doctor> GetAllDoctors();
        void AddDoctor(Doctor doctor);
    }
}
