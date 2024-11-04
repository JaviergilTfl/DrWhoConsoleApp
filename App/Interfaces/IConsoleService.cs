namespace DrWhoConsoleApp.Interfaces;
public interface IConsoleService
{
    string ReadLine();
    void WriteLine(string? message);
    void Write(string message);
}