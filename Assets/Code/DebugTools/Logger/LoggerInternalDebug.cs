using UnityEngine;

namespace Code.DebugTools.Logger
{
    internal class LoggerInternalDebug : ILoggerInternal
    {
        public string Colored(object message, Color color)
        {
            var colorString = ColorUtility.ToHtmlStringRGB(color);
            var modifiedString = $"<color=#{colorString}>{message}</color>";
            return modifiedString;
        }
        public string Bold(object message) => $"<b>{message}</b>";

        public string Cursive(object message) => $"<i>{message}</i>";

        public string AddTime(object message) => $"{message} (on time {Time.time})";

        public string AddFrameCount(object message) => $"{message} ( on frame {Time.frameCount})";

        public void InternalLog(object message, LogType logType, Object context)
        {
            switch (logType)
            {
                case LogType.Message:
                    Debug.Log(message, context);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(message, context);
                    break;
                case LogType.Error:
                    Debug.LogError(message, context);
                    break;
                default:
                    Debug.Log(message, context);
                    break;
            }
        }
    }
}