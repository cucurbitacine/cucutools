using System;
using System.Collections.Generic;
using UnityEngine;

namespace cucu.tools
{
    public class CucuLogger
    {
        public enum LogType
        {
            Log,
            Warning,
            Error,
        }

        public enum LogArea
        {
            Application,
            Editor,
            Build,
            File, // TODO write to file and make binary enum
            Nowhere,
        }

        public Color TagColor { get; private set; }
        public LogType Type { get; private set; }
        public LogArea Area { get; private set; }

        public string Tag { get; private set; }

        private static IReadOnlyDictionary<LogType, UnityEngine.LogType> _logTypeSet =
            new Dictionary<LogType, UnityEngine.LogType>
            {
                {LogType.Log, UnityEngine.LogType.Log},
                {LogType.Warning, UnityEngine.LogType.Warning},
                {LogType.Error, UnityEngine.LogType.Error},
            };

        private CucuLogger()
        {
            TagColor = Color.black;
            Type = LogType.Log;
            Area = LogArea.Application;
        }

        public static CucuLogger Create() => new CucuLogger();

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
                => Debug.unityLogger.Log(_logTypeSet[typeInternal], BuildMessage(messageInternal, tagInternal, tagColorInternal));
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

        public void Log(object message, string tag = null, Color? tagColor = null, LogType? logType = null, LogArea? logArea = null)
        {
            LogInternal(message, tag ?? Tag, tagColor ?? TagColor, logType ?? Type, logArea ?? Area);
        }

        private static string BuildMessage(object message, string tag, Color tagColor) =>
            BuildTag(tag, tagColor) + message + "\n";

        private static string BuildTag(string text, Color color) =>
            !string.IsNullOrEmpty(text) ? $"[<color=\"#{ColorToHex(color)}\">{text}</color>] : " : "";

        private static string ColorToHex(Color color)
        {
            var red = Convert.ToString((int) Mathf.Clamp(color.r * 255, 0, 255), 16);
            if (red.Length < 2) red = "0" + red;
            var green = Convert.ToString((int) Mathf.Clamp(color.g * 255, 0, 255), 16);
            if (green.Length < 2) green = "0" + green;
            var blue = Convert.ToString((int) Mathf.Clamp(color.b * 255, 0, 255), 16);
            if (blue.Length < 2) blue = "0" + blue;

            var result = red + green + blue;

            return result;
        }
    }
}