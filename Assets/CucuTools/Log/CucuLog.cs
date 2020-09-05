using System;
using UnityEngine;

namespace CucuTools
{
    public enum LogArea
    {
        Nowhere = 0, // Why am i existing?
        Editor = 1 << 0, // Log only in Editor
        Build = 1 << 1, // Log only in Build
        File = 1 << 2, // Log only in File
        EditorAndBuild = Editor | Build, // Log in Editor & Build
        EditorAndFile = Editor | File, // Log in Editor & File
        BuildAndFile = Build | File, // Log in Build  & File
        Anywhere = Editor | Build | File, // Log anywhere
    }

    [Serializable]
    public class CucuLogger
    {
        #region Static

        public static CucuLogger Global { get; }

        static CucuLogger()
        {
            Global = Create();
        }

        public static CucuLogger Create(string tag = null)
        {
            return new CucuLogger(tag);
        }
        
        #endregion

        public Guid Guid { get; }

        private CucuLogger(string tag)
        {
            Guid = Guid.NewGuid();

            Tag = tag;
            SelectedType = LogType.Log;
            SelectedArea = LogArea.Editor | LogArea.Build;
        }
        
        private CucuLogger() : this(null)
        {
        }

        #region Properties

        public string Tag
        {
            get => _tag;
            private set => _tag = value;
        }

        public LogType SelectedType
        {
            get => _selectedType;
            private set => _selectedType = value;
        }

        public LogArea SelectedArea
        {
            get => _selectedArea;
            private set => _selectedArea = value;
        }

        #endregion

        #region SerializeField

        [SerializeField] private string _tag;
        [SerializeField] private LogType _selectedType;
        [SerializeField] private LogArea _selectedArea;

        #endregion

        public void Log(
            object message,
            string tag,
            LogType type,
            LogArea logArea)
        {
#if UNITY_EDITOR
            if ((logArea & LogArea.Editor) != 0)
                if (!string.IsNullOrWhiteSpace(tag)) Debug.unityLogger.Log(type, tag, message);
                else Debug.unityLogger.Log(type, message);
#endif

#if !UNITY_EDITOR
            if ((logArea & LogArea.Build) != 0)
                if (!string.IsNullOrWhiteSpace(tag)) Debug.unityLogger.Log(type, tag, message);
                else Debug.unityLogger.Log(type, message);
#endif
            if ((logArea & LogArea.File) != 0)
            {
                // TODO log in file
            }
        }

        #region Log

        public void Log(object message)
        {
            Log(message, Tag, SelectedType, SelectedArea);
        }

        public void Log(object message, string tag)
        {
            Log(message, tag, SelectedType, SelectedArea);
        }

        #endregion

        #region LogWarning

        public void LogWarning(object message)
        {
            Log(message, Tag, LogType.Warning, SelectedArea);
        }

        public void LogWarning(object message, string tag)
        {
            Log(message, tag, LogType.Warning, SelectedArea);
        }

        #endregion

        #region LogError

        public void LogError(object message)
        {
            Log(message, Tag, LogType.Error, SelectedArea);
        }

        public void LogError(object message, string tag)
        {
            Log(message, tag, LogType.Error, SelectedArea);
        }

        #endregion

        #region Setter

        public CucuLogger SetTag(string tag)
        {
            if (Guid == Global.Guid)
            {
                Global.LogWarning("You try change Global CucuLogger");
                return this;
            }

            if (tag != null) Tag = tag;
            return this;
        }

        public CucuLogger SetType(LogType type)
        {
            if (Guid == Global.Guid)
            {
                Global.LogWarning("You try change Global CucuLogger");
                return this;
            }

            SelectedType = type;
            return this;
        }

        public CucuLogger SetArea(LogArea logArea)
        {
            if (Guid == Global.Guid)
            {
                Global.LogWarning("You try change Global CucuLogger");
                return this;
            }

            SelectedArea = logArea;
            return this;
        }

        #endregion
    }
}