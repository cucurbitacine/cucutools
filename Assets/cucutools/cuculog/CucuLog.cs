using System;
using System.Collections.Generic;
using UnityEngine;

namespace cucu.tools
{
    public class CucuLog
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

        private CucuLog(Color tagColor, LogType logType, LogArea logArea)
        {
            TagColor = tagColor;
            Type = logType;
            Area = logArea;
        }

        public static CucuLog Create() => new CucuLog(Color.black, LogType.Log, LogArea.Application);

        public static void Log(
            object message,
            string tag = null, Color? tagColor = null,
            LogType type = LogType.Log,
            LogArea area = LogArea.Application)
        {
            switch (area)
            {
                case LogArea.Application:
                    LogInternal(message, tag, tagColor, type);
                    break;
                case LogArea.Editor:
#if UNITY_EDITOR
                    LogInternal(message, tag, tagColor, type);
#endif
                    break;
                case LogArea.Build:
#if !UNITY_EDITOR
                    Log(message, tag, tagType, type);
#endif
                    break;
                case LogArea.Nowhere:
                default:
                    break;
            }
        }

        private static void LogInternal(
            object message, string tag = "",
            Color? tagColor = null,
            LogType type = LogType.Log)
        {
            Debug.unityLogger.Log(_logTypeSet[type], BuildMessage(tag, tagColor ?? Color.black, message));
        }

        public CucuLog SetTag(Color tagColor, string tag = null)
        {
            TagColor = tagColor;
            if (tag != null) Tag = tag;
            return this;
        }

        public CucuLog SetTag(string tag)
        {
            if (tag != null) Tag = tag;
            return this;
        }

        public CucuLog SetType(LogType type)
        {
            Type = type;
            return this;
        }

        public CucuLog SetArea(LogArea area)
        {
            Area = area;
            return this;
        }

        public void Log(object message, string tag)
        {
            Log(message, tag, TagColor, Type, Area);
        }

        public void Log(object message)
        {
            Log(message, Tag, TagColor, Type, Area);
        }

        private static string BuildMessage(string tag, Color tagColor, object message) =>
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