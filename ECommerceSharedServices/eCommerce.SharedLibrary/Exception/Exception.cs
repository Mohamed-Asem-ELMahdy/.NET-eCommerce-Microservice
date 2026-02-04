namespace eCommerce.SharedLibrary.Exception;
using Serilog;

public static class LogException
{
    public static void Log(System.Exception ex)
    {
        LogToFile(ex.Message);
        LogToConsole(ex.Message);
        LogToDebugger(ex.Message);
    }

    public static void LogToFile(string message) => Serilog.Log.Information(message);
    public static void LogToConsole(string msg) => Serilog.Log.Warning(msg);
    public static void LogToDebugger(string msg) => Serilog.Log.Debug(msg);
}