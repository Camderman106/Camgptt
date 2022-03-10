
namespace Camgptt.Logging
{
    public interface I6Logger : IDisposable
    {
        public void Debug(string message);
        public void Error(string message);
        public void Fatal(string message);
        public void Info(string message);
        public void Trace(string message);
        public void Warn(string message);
        public void Trace(Exception exception, string? message);
        public void Debug(Exception exception, string? message);
        public void Warn(Exception exception, string? message);
        public void Error(Exception exception, string? message);
        public void Fatal(Exception exception, string? message);
        public void Info(Exception exception, string? message);

        
    }
}