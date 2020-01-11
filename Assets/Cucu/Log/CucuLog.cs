using Cucu.Common;
using UnityEngine;

namespace Cucu.Log
{
    public enum LogArea
    {
        Application,
        Editor,
        Build,
        File, // TODO write to file and make binary enum
        Nowhere
    }

    public class CucuLogger
    {
        private CucuLogger()
        {
            TagColor = Color.black;
            Type = LogType.Log;
            LogArea = LogArea.Application;
        }

        public string Tag { get; private set; }
        public Color TagColor { get; private set; }

        public LogType Type { get; private set; }
        public LogArea LogArea { get; private set; }


        public static CucuLogger Create()
        {
            return new CucuLogger();
        }

        private static void LogInternal(
            object message,
            string tag,
            Color tagColor,
            LogType type,
            LogArea logArea)
        {
            switch (logArea)
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
                    LogInternalLocated(message, tag, tagColor, type);
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
                Debug.unityLogger.Log(typeInternal, BuildMessage(messageInternal, tagInternal, tagColorInternal));
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

        public CucuLogger SetArea(LogArea logArea)
        {
            LogArea = logArea;
            return this;
        }

        public void Log(
            object message,
            string tag = null,
            Color? tagColor = null,
            LogType? logType = null,
            LogArea? logArea = null)
        {
            LogInternal(message, tag ?? Tag, tagColor ?? TagColor, logType ?? Type, logArea ?? LogArea);
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