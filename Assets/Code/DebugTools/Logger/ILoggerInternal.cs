using UnityEngine;

namespace Code.DebugTools.Logger
{
    internal interface ILoggerInternal
    {
        public string Colored(object message, Color color) => string.Empty;
        public string Bold(object message) => string.Empty;

        public string Cursive(object message) => string.Empty;

        public string AddTime(object message) => string.Empty;

        public string AddFrameCount(object message) => string.Empty;

        public void InternalLog(object message, LogType logType, Object context) { }
    }
}