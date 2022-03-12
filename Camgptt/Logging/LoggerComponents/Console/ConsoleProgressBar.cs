using System.Diagnostics;

namespace Camgptt.Logging.LoggerComponents.ConsoleComponents
{
    /// <summary>
    /// I am fully aware this implimentation breaks the rules of inheritance and encapsulation, but I couldn't think of another way of doing it
    /// The user needs to be able to call "using progress bar" even if there is no console attached, in which case it should do nothing. 
    /// But the object still needs to be returned to the calling code so it still needs to exist
    /// Therefore I have decided to give the progress bar a nullable reference to its owner. If its owner is not null, then the progress bar asks the owner to
    /// handle the actual outputing and drawing etc. Otherwise this object exists in a non user visible state and happily ticks along anyway
    /// 
    /// Uses the IDisposable interface to force the object to die when the calling code is finished, even if somehow the TotalTicks value was wrong
    /// The progress basr doesnt die then max ticks are reached also in case the total ticks value provided was wrong. Wrong % is better than dying early
    /// 
    /// </summary>
    public class ConsoleProgressBar : IDisposable
    {
        public string Caller;
        public string Message;
        public long TotalTicks;
        public long CurrentTick;
        public Stopwatch Duration;
        public ConsoleLogger? Owner;
        private bool disposedValue;

        public ConsoleProgressBar(ConsoleLogger? owner, string caller, string message, long totalTicks)
        {
            Duration = new Stopwatch();
            Duration.Start();
            Caller = caller;
            Message = message;
            TotalTicks = totalTicks;
            CurrentTick = 0;
            Owner = owner;
        }
        public void Tick()
        {
            CurrentTick++;
            if (Owner is not null ) Owner.UpdateProgressBars();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if (Owner is not null ) Owner.KillProgressBar(this);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ConsoleProgressBar()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
