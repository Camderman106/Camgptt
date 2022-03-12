using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camgptt.Logging.LoggerComponents.ConsoleComponents
{
    public class ConsoleLogger : I6Logger, I6LoggerComponent
    {
        //I want this to be clever. If a console already exists and is attached, use that.
        //Otherwise I want to create one and make sure the std streams are linked up properly.
        //So if someone opening this in the context of a windows application (as opposed to console application) there should still be a console visible
        //For now we are just going to use the console class
        public ConsoleLogger()
        {
            
        }
        private object _lock = new object();


        //Progress Bars
        //Approach is to have a number of lines at the bottom of the Console which contain any active progress bars (nesting supported)
        //We can use the MoveBuffer command to copy them down if needed, or we can use the just control the curser to get the desired effect

        private List<ConsoleProgressBar> ProgressBars = new List<ConsoleProgressBar>();
        private bool ActiveProgressBars => ProgressBars.Count > 0;
        public ConsoleProgressBar AddProgressBar(string message, int MaxTicks, string nameOfCallingClass)
        {
            lock (_lock)
            {
                ConsoleProgressBar bar = new ConsoleProgressBar(this, nameOfCallingClass, message, MaxTicks);
                ProgressBars.Add(bar);                
                return bar;
            }            
        }
        //This is passed back up the chain which isnt good for inheritance, but i want the consoleProgressBAr to be used like an IDisposable and don't know how else to tell the logger to drop it
        public void KillProgressBar(ConsoleProgressBar bar)
        {
            if (ProgressBars.Contains(bar))
            {
                ProgressBars.Remove(bar);
                //This handles clearing and rewriting the progress bars
                WriteLine($"[{DateTime.Now}]{bar.Caller}|{bar.Message}|Finished: {String.Format("{0:P}",(float) bar.CurrentTick / bar.TotalTicks)} - {bar.Duration.ElapsedMilliseconds / 1000}s");
            }
        }


        private void WriteLine(string message)
        {
            //Normal Writeline
            if (!ActiveProgressBars)
            {
                Console.WriteLine(message); 
                return;
            }
            
            ClearProgressBars();
            //Then write the message
            Console.WriteLine(message);
            //Then rewrite the progress bars
            UpdateProgressBars();
        }

        internal void UpdateProgressBars()
        {
            //Save this current position
            (int Left, int Top) = Console.GetCursorPosition();

            //Leave a blank line between progress bars and log messages for legibility
            //First clear the line
            Console.Write(new String(' ', Console.BufferWidth - 1));
            //Return to start of the line
            Console.SetCursorPosition(0, Console.GetCursorPosition().Top); //Console.GetCursorPosition().Top is just the current row index, not the top of anything
            //Then write the message
            Console.WriteLine();

            //Loop through and populate all the progress bars on the next lines
            for (int i = ProgressBars.Count-1; i >=0; i--) //Start with the newest progress bars first
            {
                ConsoleProgressBar bar = ProgressBars[i];

                //clear the line
                Console.Write(new String(' ', Console.BufferWidth - 1));
                //return to start of the line
                Console.SetCursorPosition(0, Console.GetCursorPosition().Top);
                //Generate and write the progress bar
                string DisplayString = $"----|{bar.Caller}|{bar.Message}|{String.Format("{0:P}", (float) bar.CurrentTick / bar.TotalTicks)} - {bar.Duration.ElapsedMilliseconds / 1000}s";
                if (DisplayString.Length> Console.BufferWidth) DisplayString = DisplayString.Substring(0, Console.BufferWidth);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(DisplayString);
            }
            
            //return to the end of the log messages
            Console.SetCursorPosition(Left, Top);
        }
        private void ClearProgressBars()
        {
            //Save this current position
            (int Left, int Top) = Console.GetCursorPosition();

            //Wipe all the progress bars and the extra line
            for(int i = 0; i<ProgressBars.Count+1; i++)
            {
                Console.WriteLine(new String(' ', Console.BufferWidth - 1));
            }
            Console.SetCursorPosition(Left,Top);
        }


        //Formatting
        private string FormatMessage(string nameOfCallingClass, Severity severity, string message)
        {
            return $"[{DateTime.Now}]{nameOfCallingClass}|{severity}: {message}";
        }
        private string FormatMessage(string nameOfCallingClass, Severity severity, Exception exception, string? message)
        {
            return $"[{DateTime.Now}]{nameOfCallingClass}|{severity}: {message ?? exception.Message}";
        }

        //Assign the colors for each severity
        private Dictionary<Severity, ConsoleColor> ColorMap = new Dictionary<Severity, ConsoleColor>()
        {
            {Severity.Error, ConsoleColor.Red},
            {Severity.Fatal, ConsoleColor.DarkRed},
            {Severity.Info, ConsoleColor.White},
            {Severity.Debug, ConsoleColor.DarkYellow},
            {Severity.Trace, ConsoleColor.Gray},
            {Severity.Warn, ConsoleColor.Yellow }
        };

        
        //ILogger Interface for direct/internal use
        public void Debug(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Debug];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Debug, message));
                Console.ResetColor();
            }
        }

        public void Error(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Error];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Error, message));
                Console.ResetColor();
            }

        }

        public void Fatal(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Fatal];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Fatal, message));
                Console.ResetColor();
            }
        }

        public void Info(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Info];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Info, message));
                Console.ResetColor();
            }
        }

        public void Trace(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Trace];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Trace, message));
                Console.ResetColor();
            }
        }

        public void Warn(string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Warn];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Warn, message));
                Console.ResetColor();
            }
        }
        public void Trace(Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Trace];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Trace, exception, message));
                Console.ResetColor();
            }
        }

        public void Debug(Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Debug];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Debug, exception, message));
                Console.ResetColor();
            }
        }

        public void Warn(Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Warn];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Warn, exception, message));
                Console.ResetColor();
            }
        }

        public void Error(Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Error];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Error, exception, message));
                Console.ResetColor();
            }
        }

        public void Fatal(Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Fatal];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Fatal, exception, message));
                Console.ResetColor();
            }
        }

        public void Info(Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Info];
                WriteLine(FormatMessage(I6LoggerComponent.NameOfCallingClass(), Severity.Info, exception, message));
                Console.ResetColor();
            }
        } 
    


        //ILoggerComponent Interface for control from the main logger class
        void I6LoggerComponent.Info(string nameOfCallingClass, string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Info];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Info, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Info(string nameOfCallingClass, Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Info];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Info, exception, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Fatal(string nameOfCallingClass, string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Fatal];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Fatal, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Fatal(string nameOfCallingClass, Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Fatal];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Fatal, exception, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Error(string nameOfCallingClass, string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Error];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Error, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Error(string nameOfCallingClass, Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Error];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Error, exception, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Warn(string nameOfCallingClass, string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Warn];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Warn, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Warn(string nameOfCallingClass, Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Warn];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Warn, exception, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Debug(string nameOfCallingClass, string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Debug];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Debug, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Debug(string nameOfCallingClass, Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Debug];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Debug, exception, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Trace(string nameOfCallingClass, string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Trace];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Trace, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Trace(string nameOfCallingClass, Exception exception, string? message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Trace];
                WriteLine(FormatMessage(nameOfCallingClass, Severity.Trace, exception, message));
                Console.ResetColor();
            }
        }

        void I6LoggerComponent.Raw(string nameOfCallingClass, string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = ColorMap[Severity.Info];
                Console.Write(message);
                Console.ResetColor();
            }
        }
                
        //IDisposable

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            //Managed resources are those that are pure .NET code and managed by the runtime and are under its direct control.
            //Unmanaged resources are those that are not. File handles, pinned memory, COM objects, database connections etc.
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        //~ConsoleLogger()
        //{
        //    // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //    Dispose(disposing: false);
        //}

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
}
