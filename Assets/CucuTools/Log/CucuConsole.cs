using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    /// <summary>
    ///   A console to display Unity's debug logs in-game.
    /// </summary>
    [DisallowMultipleComponent]
    public class CucuConsole : MonoBehaviour
    {
        private const int Margin = 20;

        // Visual elements:

        private static readonly Dictionary<LogType, Color> LogTypeColors = new Dictionary<LogType, Color>
        {
            {LogType.Assert, Color.white},
            {LogType.Error, Color.red},
            {LogType.Exception, Color.red},
            {LogType.Log, Color.white},
            {LogType.Warning, Color.yellow}
        };

        private readonly GUIContent _clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        private readonly GUIContent _collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

        private readonly List<Log> _logs = new List<Log>();
        private readonly Rect _titleBarRect = new Rect(0, 0, 10000, 20);

        /// <summary>
        ///   The hotkey to show and hide the console window.
        /// </summary>
        [Header("Ctrl + ")] [SerializeField] private KeyCode _toggleKey = KeyCode.BackQuote;
        [Header("Try clear Cucu useless log")] [SerializeField] private bool _clearCucuUselessLog = true;

        private bool _collapse;
        private Vector2 _scrollPosition;
        private bool _show;

        private Rect _windowRect; 

        private CucuConsole()
        {
        }

        private static CucuConsole Instance { get; set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void Update()
        {
            if ((_show || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) &&
                Input.GetKeyDown(_toggleKey)) _show = !_show;
        }

        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            if (!_show) return;

            _windowRect = new Rect(Margin, Margin, Screen.width - Margin * 2, Screen.height - Margin * 2);
            GUILayout.Window(123456, _windowRect, ConsoleWindow, "Console");
        }

        /// <summary>
        ///   A window that displays the recorded logs.
        /// </summary>
        /// <param name="windowId">Window ID.</param>
        private void ConsoleWindow(int windowId)
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            // Iterate through the recorded logs.
            for (var i = 0; i < _logs.Count; i++)
            {
                var log = _logs[i];

                // Combine identical messages if collapse option is chosen.
                if (_collapse)
                {
                    var messageSameAsPrevious = i > 0 && log.Message == _logs[i - 1].Message;

                    if (messageSameAsPrevious) continue;
                }

                GUI.contentColor = LogTypeColors[log.Type];
                GUILayout.Label($"\n[{log.Time.ToString(CultureInfo.CurrentCulture)}] {log.Message}");

                if (log.Type != LogType.Error && log.Type != LogType.Exception) continue;

                GUI.contentColor = new Color(0.8f, 0.1f, 0.1f, 1f);
                var stackTrace = log.StackTrace.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

                if (_clearCucuUselessLog)
                {
                    stackTrace = stackTrace
                        .Where(s => 
                            !s.Contains("CucuTools.Cucu:Log") &&
                            !s.Contains("CucuTools.CucuLogger:") &&
                            !s.Contains("UnityEngine.Logger"))
                        .ToArray();
                }
                
                //for (var index = stackTrace.Length - 1; index >= 0; index--)
                for (var index = 0; index < stackTrace.Length; index++)
                    GUILayout.Label($"\t[{stackTrace.Length - 1 - index}] {stackTrace[index]}");
            }

            GUILayout.EndScrollView();

            GUI.contentColor = Color.white;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(_clearLabel)) _logs.Clear();

            _collapse = GUILayout.Toggle(_collapse, _collapseLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();

            // Allow the window to be dragged by its title bar.
            GUI.DragWindow(_titleBarRect);
        }

        /// <summary>
        ///   Records a log from the log callback.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="stackTrace">Trace of where the message came from.</param>
        /// <param name="type">Type of message (error, exception, warning, assert).</param>
        private void HandleLog(string message, string stackTrace, LogType type)
        {
            var time = DateTime.Now;
            _logs.Add(new Log
            {
                Message = message,
                StackTrace = stackTrace,
                Type = type,
                Time = time
            });
        }

        private struct Log
        {
            public string Message;
            public string StackTrace;
            public LogType Type;
            public DateTime Time;
        }
    }
}