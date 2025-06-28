using System;
using System.Runtime.CompilerServices;

public class Logger : IEngineComponent
{
    #region Interface
    public static Logger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Logger();
            }

            return _instance;
        }
    }
    private static Logger _instance;

    public IEngineComponent Init()
    {
        ChangeMinLogLevel(LogLevel.Debug);
        return this;
    }
    #endregion

    private static LogLevel _minLogLevel = LogLevel.Verbose;
    private static int _logCountInFrame = 0;
    private static int _logDepth = 0;

    public enum LogLevel
    {
        Verbose,
        Debug,
        Warning,
        Error,
        Fatal,
    }

    public static void ChangeMinLogLevel(LogLevel targetLevel)
    {
        _minLogLevel = targetLevel;
    }

    public static void Verbose(object message, Type sender = null, [CallerMemberName] string caller = "", bool formatLog = true)
    {
        Log(LogLevel.Verbose, message.ToString(), sender, caller, formatLog);
    }

    public static void Debug(object message, Type sender = null, [CallerMemberName] string caller = "", bool formatLog = true)
    {
        Log(LogLevel.Debug, message.ToString(), sender, caller, formatLog);
    }

    public static void Warning(object message, Type sender, [CallerMemberName] string caller = "", bool formatLog = true)
    {
        Log(LogLevel.Warning, message.ToString(), sender, caller, formatLog);
    }

    public static void Error(object message, Type sender, [CallerMemberName] string caller = "", bool formatLog = true)
    {
        Log(LogLevel.Error, message.ToString(), sender, caller, formatLog);
    }

    public static void Fatal(object message, Type sender, [CallerMemberName] string caller = "", bool formatLog = true)
    {
        Log(LogLevel.Fatal, message.ToString(), sender, caller, formatLog);
    }

    public static void DepthUp()
    {
        _logDepth++;
    }

    public static void DepthDown()
    {
        _logDepth--;
    }

    private static void Log(LogLevel level, string message, Type sender, string caller, bool formatLog = true)
    {
        if (level >= _minLogLevel)
        {
            ++_logCountInFrame;
        }

        string logMessage = formatLog ? $"{level} in {sender?.Name ?? "Unknown"}.{caller}() : {message}" : message;

        LogFile(level, logMessage);
#if UNITY_EDITOR
        LogConsole(level, logMessage);
#endif
    }

    private static void LogFile(LogLevel level, string message)
    {
        //// TODO : Implement this;
    }

    private static void LogConsole(LogLevel level, string message)
    {
        if (level < _minLogLevel)
        {
            return;
        }

        message = new string('\t', _logDepth) + message;

        switch (level)
        {
            case LogLevel.Verbose:
            case LogLevel.Debug:
                UnityEngine.Debug.Log(message);
                break;

            case LogLevel.Warning:
                UnityEngine.Debug.LogWarning(message);
                break;

            case LogLevel.Error:
            case LogLevel.Fatal:
                UnityEngine.Debug.LogError(message);
                break;
        }
    }

    public void ECLateUpdate()
    {
        if (_logCountInFrame > 0)
        {
            switch (_minLogLevel)
            {
                case LogLevel.Verbose:
                    Verbose($"------------------------------------", formatLog: false);
                    break;

                case LogLevel.Debug:
                    Debug($"------------------------------------", formatLog: false);
                    break;

                case LogLevel.Warning:
                    Warning($"------------------------------------", null, formatLog: false);
                    break;

                case LogLevel.Error:
                    Error($"------------------------------------", null, formatLog: false);
                    break;

                case LogLevel.Fatal:
                    Error($"------------------------------------", null, formatLog: false);
                    break;
            }

            _logCountInFrame = 0;
        }
    }
}
