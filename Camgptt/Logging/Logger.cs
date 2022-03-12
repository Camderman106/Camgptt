using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Camgptt.Logging.LoggerComponents.ConsoleComponents;
using Camgptt.Logging.LoggingComponents;
using Camgptt.Logging.LoggingComponents.DB;
using Camgptt.Logging.LoggingComponents.File;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Camgptt.Logging;

public sealed class Logger : I6Logger, IDisposable
{
    //Singleton Setup
    //Lazy<> is thread safe
    private static readonly Lazy<Logger> Lazy = new Lazy<Logger>(() => new Logger());
    public static Logger GetLogger() { return Lazy.Value; }
    private Logger() { }
    private object _lock = new object();

    private Severity mininumSeverity = Severity.Trace;
    public Severity MininumSeverity
    {
        get
        {
            return mininumSeverity;
        }
        set
        {
            mininumSeverity = value;
            Info($"Minimum log severity set to {mininumSeverity}");
        }
    }
    //For iterating over the outputs
    private List<I6LoggerComponent> Outputs = new List<I6LoggerComponent>();
    
    private bool ConsoleAttached, FileLogAttached, MongoDBAttached;
    private ConsoleLogger? ConsoleLogger;
    
    //Psudo-Builder Methods. These should only ever be called once at the start of the program.
    //Because we have no way of directing different logs at different files/dbs the number of each type of output is limited to 1
    public Logger AttachFileLogger(string LogFilePath = "log.txt", FileMode mode = FileMode.Append, FileAccess access = FileAccess.Write, FileShare share = FileShare.Read)
    {
        lock (_lock)
        {
            if (!FileLogAttached)
            {
                try
                {
                    var fl = new FileLogger(LogFilePath, mode, access, share);
                    GetLogger().Outputs.Add(fl);
                    FileLogAttached = true;
                }
                catch (Exception ex)
                {
                    Error(ex, $"Unable to create FileLogger at {LogFilePath}\n{ex.Message}");
                }
            }
            else
            {
                Error($"A file logger already exists. {LogFilePath}");
            }
            
        }
        return GetLogger();
    }
    public ConsoleProgressBar AddProgressBar(string message, int maxTicks)
    {
        if (ConsoleLogger is not null) 
            return ConsoleLogger.AddProgressBar(message, maxTicks, I6LoggerComponent.NameOfCallingClass());
        else 
            return new ConsoleProgressBar(null, I6LoggerComponent.NameOfCallingClass(), message,maxTicks);
        
    }
    public Logger AttachConsoleLogger() 
    {
        lock (_lock)
        {
            if (!ConsoleAttached)
            {
                try
                {
                    var cl = new ConsoleLogger();
                    GetLogger().Outputs.Add(cl);
                    ConsoleAttached = true;
                    ConsoleLogger = cl;
                }
                catch (Exception ex)
                {
                    Error(ex, $"Unable to create console logger\n{ex.Message}");
                }
            }
            else
            {
                Error("A console is already attached");
            }
        }
        return GetLogger();
    }
    public Logger AttachDBLogger(string? connectionString = null)
    {
        lock (_lock)
        {
            if(!MongoDBAttached)
            {
                try
                {
                    var dbl = new MongoDBLogger(connectionString);
                    GetLogger().Outputs.Add(dbl);
                    MongoDBAttached = true;
                }
                catch (Exception ex)
                {
                    Error(ex, $"Unable to add DB logger\n{ex.Message}");
                }
            }
            else
            {
                Error($"A DB is already in use \"{connectionString}\"");
            }
            
        }
        return GetLogger();
    }

    //Main Body of Log Methods. ILogger Interface
    public void Debug(string message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Debug) foreach (var logger in Outputs) { logger.Debug(I6LoggerComponent.NameOfCallingClass(), message); }
        }
    }

    public void Error(string message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Error) foreach (var logger in Outputs) { logger.Error(I6LoggerComponent.NameOfCallingClass(), message); }
        }
    }

    public void Fatal(string message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Fatal) foreach (var logger in Outputs) { logger.Fatal(I6LoggerComponent.NameOfCallingClass(), message); }
        }
    }

    public void Info(string message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Info) foreach (var logger in Outputs) { logger.Info(I6LoggerComponent.NameOfCallingClass(), message); }
        }
    }

    public void Trace(string message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Trace) foreach (var logger in Outputs) { logger.Trace(I6LoggerComponent.NameOfCallingClass(), message); }
        }
    }

    public void Warn(string message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Warn) foreach (var logger in Outputs) { logger.Warn(I6LoggerComponent.NameOfCallingClass(), message); }
        }
    }
    public void Trace(Exception exception, string? message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Trace) foreach (var logger in Outputs) { logger.Trace(I6LoggerComponent.NameOfCallingClass(), exception, message); }
        }
    }

    public void Debug(Exception exception, string? message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Debug) foreach (var logger in Outputs) { logger.Debug(I6LoggerComponent.NameOfCallingClass(), exception, message); }
        }
    }

    public void Warn(Exception exception, string? message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Warn) foreach (var logger in Outputs) { logger.Warn(I6LoggerComponent.NameOfCallingClass(), exception, message); }
        }
    }

    public void Error(Exception exception, string? message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Error) foreach (var logger in Outputs) { logger.Error(I6LoggerComponent.NameOfCallingClass(), exception, message); }
        }
    }

    public void Fatal(Exception exception, string? message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Fatal) foreach (var logger in Outputs) { logger.Fatal(I6LoggerComponent.NameOfCallingClass(), exception, message); }
        }
    }

    public void Info(Exception exception, string? message)
    {
        lock (_lock)
        {
            if (MininumSeverity <= Severity.Info) foreach (var logger in Outputs) { logger.Info(I6LoggerComponent.NameOfCallingClass(), exception, message); }
        }
    }

    
    //Dispose Pattern
    private bool disposedValue;
    private void Dispose(bool disposing)
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
            foreach(var output in Outputs) output.Dispose();
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~Logger()
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

public enum Severity
{
    Trace,
    Debug,
    Warn,
    Error,
    Fatal,
    Info
}


