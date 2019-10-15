using System;
using System.Collections.Generic;
using UnityEngine;

namespace cucu.tools
{
    public class CucuLog
    {
        public TagParams CurrentTagParams { get; set; }

        public LogParams CurrentLogParams { get; set; }

        public static void Log(LogArgs args)
        {
            switch (args.logParams.logArea)
            {
                case LogParams.LogArea.Anywhere:
                    args.Log();
                    break;
                case LogParams.LogArea.Editor:
#if UNITY_EDITOR
                    args.Log();
#endif
                    break;
                case LogParams.LogArea.Build:
#if UNITY_STANDALONE
                    args.Log();
#endif
                    break;
                default:
                    break;
            }
        }

        public CucuLog(TagParams tagParams = new TagParams(), LogParams logParams = new LogParams())
        {
            CurrentTagParams = tagParams;
            CurrentLogParams = logParams;
        }

        public static void Log(object message, TagParams? tagParams = null, LogParams? logParams = null)
        {
            var args = new LogArgs {message = message};

            if (tagParams != null) args.tagParams = tagParams.Value;
            if (logParams != null) args.logParams = logParams.Value;

            Log(args);
        }

        public void Log(object message, LogParams logParams)
        {
            Log(message, CurrentTagParams, logParams);
        }

        public void Log(object message, TagParams tagParams)
        {
            Log(message, tagParams, CurrentLogParams);
        }

        public void Log(object message)
        {
            Log(message, CurrentTagParams, CurrentLogParams);
        }

        public class LogArgs
        {
            public object message;
            public TagParams tagParams;
            public LogParams logParams;

            public LogArgs()
            {
                message = "";
                tagParams = new TagParams();
                logParams = new LogParams();
            }

            public void Log()
            {
                if (tagParams.Type != TagParams.TagType.None)
                    Debug.unityLogger.Log(logParams.GetUnityLogType(), tagParams.TextFormatted, message);
                else
                    Debug.unityLogger.Log(logParams.GetUnityLogType(), message);
            }
        }

        public struct LogParams
        {
            public enum LogType
            {
                Default,
                Warning,
                Error,
            }

            public enum LogArea
            {
                Anywhere,
                Editor,
                Build,
                Nowhere,
            }

            public LogType logType;
            public LogArea logArea;

            private readonly IReadOnlyDictionary<LogType, UnityEngine.LogType> _logTypeSet;

            public LogParams(LogType type, LogArea area)
            {
                logType = type;
                logArea = area;

                _logTypeSet =
                    new Dictionary<LogType, UnityEngine.LogType>
                    {
                        {LogType.Default, UnityEngine.LogType.Log},
                        {LogType.Warning, UnityEngine.LogType.Warning},
                        {LogType.Error, UnityEngine.LogType.Error},
                    };
            }

            public UnityEngine.LogType GetUnityLogType(LogType? type = null)
            {
                var sType = type ?? logType;
                return _logTypeSet.ContainsKey(sType) ? _logTypeSet[sType] : UnityEngine.LogType.Log;
            }
        }

        public struct TagParams
        {
            public enum TagType
            {
                None,
                Info,
                Success,
                Fail,
            }

            public TagType Type { get; set; }

            public string Text
            {
                get => _text;
                set => _text = value;
            }

            public string TextFormatted => GetTextFormatted(Text, Type);

            private string _text;

            private static readonly IReadOnlyDictionary<TagType, string> _colorSet = new Dictionary<TagType, string>
            {
                {TagType.None, "black"},
                {TagType.Info, "yellow"},
                {TagType.Success, "green"},
                {TagType.Fail, "red"},
            };

            public TagParams(TagType type, string text = "")
            {
                Type = type;
                _text = text;
            }

            public static string GetTextFormatted(string text, TagType type = TagType.Info)
            {
                return $"[<color={_colorSet[type]}>{text}</color>]";
            }
        }
    }
}