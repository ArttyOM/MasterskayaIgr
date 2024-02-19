using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.DebugTools.Logger
{
    public static class Logger
    {
        private static readonly ILoggerInternal InternalLogger;

        static Logger()
        {
            #if DEBUG
            InternalLogger = new LoggerInternalDebug();
            #else
            InternalLogger = new LoggerInternalNoDebug();
            #endif
        }
        public static void Mark()
        {
            const string message = "(_MARK_)";
            Log(message, LogType.Message, null);
        }

        public static void Separator()
        {
            const string message = "============================================";
            Log(message, LogType.Message, null);
        }

        public static void Log(this object message, Object context = null)
        {
            Log(message, LogType.Message, context);
        }

        public static void LogWarning(this object message, Object context = null)
        {
            Log(message, LogType.Warning, context);
        }

        public static void LogError(this object message, Object context = null)
        {
            Log(message, LogType.Error, context);
        }

        public static string Colored(this object message, Color color) => InternalLogger.Colored(message, color);

        public static string Bold(this object message) => InternalLogger.Bold(message);

        public static string Cursive(this object message) => InternalLogger.Cursive(message);

        public static string AddTime(this object message) => InternalLogger.AddTime(message);

        public static string AddFrameCount(this object message) => InternalLogger.AddFrameCount(message);

        private static void Log(this object message, LogType logType, Object context) => InternalLogger.InternalLog(message, logType, context);
    }
}