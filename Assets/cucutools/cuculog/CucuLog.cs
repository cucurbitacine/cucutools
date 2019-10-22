using System.Collections.Generic;
using UnityEngine;

namespace cucu.tools
{
    public class CucuLogger
    {
        public enum LogArea
        {
            Application,
            Editor,
            Build,
            File, // TODO write to file and make binary enum
            Nowhere
        }

        public enum LogType
        {
            Log,
            Warning,
            Error
        }

        private static readonly IReadOnlyDictionary<LogType, UnityEngine.LogType> _logTypeSet =
            new Dictionary<LogType, UnityEngine.LogType>
            {
                {LogType.Log, UnityEngine.LogType.Log},
                {LogType.Warning, UnityEngine.LogType.Warning},
                {LogType.Error, UnityEngine.LogType.Error}
            };

        private CucuLogger()
        {
            TagColor = Color.black;
            Type = LogType.Log;
            Area = LogArea.Application;
        }

        public Color TagColor { get; private set; }
        public LogType Type { get; private set; }
        public LogArea Area { get; private set; }

        public string Tag { get; private set; }

        public static CucuLogger Create()
        {
            return new CucuLogger();
        }

        private static void LogInternal(
            object message,
            string tag,
            Color tagColor,
            LogType type,
            LogArea area)
        {
            switch (area)
            {
                case LogArea.Application:
                    LogInternalLocated(message, tag, tagColor, type);
                    break;
                case LogArea.Editor:
#if UNITY_EDITOR
                    LogInternalLocated(message, tag, tagColor, type);
#endif
                    break;
                case LogArea.Build:
#if !UNITY_EDITOR
                    LogInternalLocated(message, tag, tagType, type);
#endif
                    break;
                case LogArea.Nowhere:
                default:
                    break;
            }

            void LogInternalLocated(
                object messageInternal,
                string tagInternal,
                Color tagColorInternal,
                LogType typeInternal)
            {
                Debug.unityLogger.Log(_logTypeSet[typeInternal],
                    BuildMessage(messageInternal, tagInternal, tagColorInternal));
            }
        }

        public CucuLogger SetTag(Color tagColor, string tag = null)
        {
            TagColor = tagColor;
            if (tag != null) Tag = tag;
            return this;
        }

        public CucuLogger SetTag(string tag)
        {
            if (tag != null) Tag = tag;
            return this;
        }

        public CucuLogger SetType(LogType type)
        {
            Type = type;
            return this;
        }

        public CucuLogger SetArea(LogArea area)
        {
            Area = area;
            return this;
        }

        public void Log(
            object message,
            string tag = null,
            Color? tagColor = null,
            LogType? logType = null,
            LogArea? logArea = null)
        {
            LogInternal(message, tag ?? Tag, tagColor ?? TagColor, logType ?? Type, logArea ?? Area);
        }

        private static string BuildMessage(object message, string tag, Color tagColor)
        {
            return BuildTag(tag, tagColor) + message;
        }

        private static string BuildTag(string text, Color color)
        {
            return !string.IsNullOrEmpty(text) ? $"[{text.SetColor(color)}] : " : "";
        }
    }
}