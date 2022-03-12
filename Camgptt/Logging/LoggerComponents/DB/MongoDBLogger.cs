using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Camgptt.Logging.LoggingComponents.DB;

internal class MongoDBLogger : I6Logger , I6LoggerComponent
{
    private MongoClient _client;
    private IMongoDatabase _database;
    private IMongoCollection<MongoLogRecord> _collection;
    private string _databaseName = "camgptt";
    private string _collectionName = "logs";
    private string defaultConnectionString = "mongodb://localhost:27017";
    private bool _connected = false;

    public MongoDBLogger(string? connectionString)
    {
        _client = new MongoClient(connectionString?? defaultConnectionString);
        _database = _client.GetDatabase(_databaseName);
        if (_database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait(1000)) 
        { 
            _connected = true; 
            _collection = _database.GetCollection<MongoLogRecord>(_collectionName);
        }
        else
        {
            throw new MongoClientException($"Could not connect to the database: {connectionString}");
        }
    }


    void I6Logger.Debug(string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message, I6LoggerComponent.NameOfCallingClass(), Severity.Debug));
    }

    void I6Logger.Error(string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message, I6LoggerComponent.NameOfCallingClass(), Severity.Error));
    }

    void I6Logger.Fatal(string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message, I6LoggerComponent.NameOfCallingClass(), Severity.Fatal));
    }

    void I6Logger.Info(string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message, I6LoggerComponent.NameOfCallingClass(), Severity.Info));
    }

    void I6Logger.Trace(string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message, I6LoggerComponent.NameOfCallingClass(), Severity.Trace));
    }

    void I6Logger.Warn(string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message, I6LoggerComponent.NameOfCallingClass(), Severity.Warn));
    }

    void I6Logger.Trace(Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", I6LoggerComponent.NameOfCallingClass(), Severity.Trace, exception));
    }

    void I6Logger.Debug(Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", I6LoggerComponent.NameOfCallingClass(), Severity.Debug, exception));
    }

    void I6Logger.Warn(Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", I6LoggerComponent.NameOfCallingClass(), Severity.Warn, exception));
    }

    void I6Logger.Error(Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", I6LoggerComponent.NameOfCallingClass(), Severity.Error, exception));
    }

    void I6Logger.Fatal(Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", I6LoggerComponent.NameOfCallingClass(), Severity.Fatal, exception));
    }

    void I6Logger.Info(Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", I6LoggerComponent.NameOfCallingClass(), Severity.Info, exception));
    }


    void I6LoggerComponent.Info(string nameOfCallingClass, string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Info));
    }

    void I6LoggerComponent.Info(string nameOfCallingClass, Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Info, exception));
    }

    void I6LoggerComponent.Fatal(string nameOfCallingClass, string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Fatal));
    }

    void I6LoggerComponent.Fatal(string nameOfCallingClass, Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Fatal, exception));
    }

    void I6LoggerComponent.Error(string nameOfCallingClass, string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Error));
    }

    void I6LoggerComponent.Error(string nameOfCallingClass, Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Error, exception));
    }

    void I6LoggerComponent.Warn(string nameOfCallingClass, string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Warn));
    }

    void I6LoggerComponent.Warn(string nameOfCallingClass, Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Warn, exception));
    }

    void I6LoggerComponent.Debug(string nameOfCallingClass, string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Debug));
    }

    void I6LoggerComponent.Debug(string nameOfCallingClass, Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Debug, exception));
    }

    void I6LoggerComponent.Trace(string nameOfCallingClass, string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Trace));
    }

    void I6LoggerComponent.Trace(string nameOfCallingClass, Exception exception, string? message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message ?? "", nameOfCallingClass, Severity.Trace, exception));

    }

    void I6LoggerComponent.Raw(string nameOfcallingClass, string message)
    {
        if (_connected) _collection.InsertOne(new MongoLogRecord(message, nameOfcallingClass, Severity.Info));
    }


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
    // ~MongoDBLogger()
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

internal class MongoLogRecord
{
    public MongoLogRecord(string message, string @class, Severity severity)
    {
        DateTime = DateTime.Now;
        Message = message;
        Class = @class;
        Severity = severity;
    }

    public MongoLogRecord(string message, string @class, Severity severity, Exception attachedException)
    {
        DateTime = DateTime.Now;
        Message = message;
        Class = @class;
        AttachedException = attachedException;
        Severity = severity;
    }

    public DateTime DateTime { get; set; }
    public string Message { get; set; }
    public string Class { get; set; }
    public Exception? AttachedException { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public Severity Severity { get; set; }
    
}