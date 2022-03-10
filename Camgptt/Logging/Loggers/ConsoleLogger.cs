using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camgptt.Logging.Loggers
{
    internal class ConsoleLogger : I6Logger, I6LoggerComponent
    {
        //I want this to be clever. If a console already exists and is attached, use that.
        //Otherwise I want to create one and make sure the std streams are linked up properly.
        //So if someone opening this in the context of a windows application (as opposed to console application) there should still be a console visible
        public ConsoleLogger()
        {
            
        }
        //Formatting
        private string FormatMessage( string nameOfCallingClass, Severity severity, string message)
        {
            return $"[{DateTime.Now}]{nameOfCallingClass}|{severity}: {message}";
        }
        private string FormatMessage(string nameOfCallingClass, Severity severity, Exception exception, string? message)
        {
            return $"[{DateTime.Now}]{nameOfCallingClass}|{severity}: {message ?? exception.Message}";
        }
        //ILogger
        void I6Logger.Debug(string message)
        {
            Console.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Debug, message));
            _writer.Flush();
        }

        void I6Logger.Error(string message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Error, message));
            _writer.Flush();
        }

        void I6Logger.Fatal(string message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Fatal, message));
            _writer.Flush();
        }

        void I6Logger.Info(string message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Info, message));
            _writer.Flush();
        }

        void I6Logger.Trace(string message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Trace, message));
            _writer.Flush();
        }

        void I6Logger.Warn(string message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Warn, message));
            _writer.Flush();
        }
        void I6Logger.Trace(Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Trace, exception, message));
            _writer.Flush();
        }

        void I6Logger.Debug(Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Debug, exception, message));
            _writer.Flush();
        }

        void I6Logger.Warn(Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Warn, exception, message));
            _writer.Flush();
        }

        void I6Logger.Error(Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Error, exception, message));
            _writer.Flush();
        }

        void I6Logger.Fatal(Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Fatal, exception, message));
            _writer.Flush();
        }

        void I6Logger.Info(Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Info, exception, message));
            _writer.Flush();
        }


        //I6SubLogger
        void I6LoggerComponent.Info(string nameOfCallingClass, string message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Info, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Info(string nameOfCallingClass, Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Info, exception, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Fatal(string nameOfCallingClass, string message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Fatal, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Fatal(string nameOfCallingClass, Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Fatal, exception, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Error(string nameOfCallingClass, string message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Error, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Error(string nameOfCallingClass, Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Error, exception, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Warn(string nameOfCallingClass, string message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Warn, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Warn(string nameOfCallingClass, Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Warn, exception, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Debug(string nameOfCallingClass, string message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Debug, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Debug(string nameOfCallingClass, Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Debug, exception, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Trace(string nameOfCallingClass, string message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Trace, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Trace(string nameOfCallingClass, Exception exception, string? message)
        {
            _writer.WriteLine(FormatMessage(nameOfCallingClass, Severity.Trace, exception, message));
            _writer.Flush();
        }

        void I6LoggerComponent.Raw(string nameOfCallingClass, string message)
        {
            _writer.Write(message);
            _writer.Flush();
        }



        
        //IDisposable

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                _writer.Dispose();
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~ConsoleLogger()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
}
