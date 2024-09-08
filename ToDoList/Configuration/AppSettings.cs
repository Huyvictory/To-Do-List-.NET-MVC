namespace ToDoList.Configuration;

public class AppSettings
{
    public DatabaseSettings Database { get; set; } = new();
    public LoggingSettings Logging { get; set; } = new();
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; }
    public int MaxRetryCount { get; set; }
    public int CommandTimeout { get; set; }
}

public class LoggingSettings
{
    public Dictionary<string, string> LogLevel { get; set; } = new();
}