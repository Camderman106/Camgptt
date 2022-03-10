using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

    public Severity MininumSeverity = Severity.Trace;
    private HashSet<I6LoggerComponent> Outputs = new HashSet<I6LoggerComponent>();
    private bool ConsoleAttached;
    
    

    public Logger AttachFileLogger(string LogFilePath, FileMode mode = FileMode.Append, FileAccess access = FileAccess.Write, FileShare share = FileShare.Read)
    {
        try
        {
            var fl = new FileLogger(LogFilePath, mode, access, share);
            GetLogger().Outputs.Add(fl);
        }
        catch (Exception ex)
        {
            Error(ex, $"Unable to create FileLogger at {LogFilePath}\n{ex.Message}");
        }
        return GetLogger();
    }
    //public Logger AttachDBLogger() { };

    //Main Body
    public void Debug(string message)
    {
        if(MininumSeverity>=Severity.Debug) foreach(var logger in Outputs) { logger.Debug(I6LoggerComponent.NameOfCallingClass(), message); }
    }

    public void Error(string message)
    {
        if (MininumSeverity >= Severity.Error) foreach (var logger in Outputs) { logger.Error(I6LoggerComponent.NameOfCallingClass(), message); }
    }

    public void Fatal(string message)
    {
        if (MininumSeverity >= Severity.Fatal) foreach ( var logger in Outputs) { logger.Fatal(I6LoggerComponent.NameOfCallingClass(), message); }
    }

    public void Info(string message)
    {
        if (MininumSeverity >= Severity.Info) foreach (var logger in Outputs) { logger.Info(I6LoggerComponent.NameOfCallingClass(), message); }
    }

    public void Trace(string message)
    {
        if (MininumSeverity >= Severity.Trace) foreach (var logger in Outputs) { logger.Trace(I6LoggerComponent.NameOfCallingClass(), message); }
    }

    public void Warn(string message)
    {
        if (MininumSeverity >= Severity.Warn) foreach (var logger in Outputs) { logger.Warn(I6LoggerComponent.NameOfCallingClass(), message); }
    }
    public void Trace(Exception exception, string? message)
    {
        if (MininumSeverity >= Severity.Trace) foreach (var logger in Outputs) { logger.Trace(I6LoggerComponent.NameOfCallingClass(), exception, message); }
    }

    public void Debug(Exception exception, string? message)
    {
        if (MininumSeverity >= Severity.Debug) foreach (var logger in Outputs) { logger.Debug(I6LoggerComponent.NameOfCallingClass(), exception, message); }
    }

    public void Warn(Exception exception, string? message)
    {
        if (MininumSeverity >= Severity.Warn) foreach (var logger in Outputs) { logger.Warn(I6LoggerComponent.NameOfCallingClass(), exception, message); }
    }

    public void Error(Exception exception, string? message)
    {
        if (MininumSeverity >= Severity.Error) foreach (var logger in Outputs) { logger.Error(I6LoggerComponent.NameOfCallingClass(), exception, message); }
    }

    public void Fatal(Exception exception, string? message)
    {
        if (MininumSeverity >= Severity.Fatal) foreach (var logger in Outputs) { logger.Fatal(I6LoggerComponent.NameOfCallingClass(), exception, message); }
    }

    public void Info(Exception exception, string? message)
    {
        if (MininumSeverity >= Severity.Info) foreach (var logger in Outputs) { logger.Info(I6LoggerComponent.NameOfCallingClass(), exception, message); }
    }

    

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


