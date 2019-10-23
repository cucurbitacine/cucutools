using System;
using cucu.tools;
using UnityEngine;
using Random = System.Random;

namespace cucu.example
{
    public class ExampleCucuLog : MonoBehaviour
    {
        private readonly Color[] colors = new[]
        {
            Color.red, Color.yellow.LerpTo(Color.red), Color.yellow, Color.green, Color.cyan,
            Color.blue, Color.magenta
        };

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) Do();
        }

        private void Do()
        {
            DebugLog();

            CucuLog();

            CucuLoggerLog();

            Error();
        }

        private void DebugLog()
        {
            Debug.Log("[Debug] : Log", this);
            Debug.Assert(true, "[Debug] : Assert-true", this);
            Debug.Assert(false, "[Debug] : Assert-false", this);
            Debug.LogError("[Debug] : LogError", this);
            Debug.LogException(new Exception("[Debug] : LogException"), this);
            Debug.LogWarning("[Debug] : LogWarning", this);
        }

        private void CucuLog()
        {
            Cucu.Logger.SetTag("Cucu.Logger");

            Cucu.Log("Log", logType: LogType.Log);
            Cucu.Log("Assert", logType: LogType.Assert);
            Cucu.Log("Error", logType: LogType.Error);
            Cucu.Log("Exception", logType: LogType.Exception);
            Cucu.Log("Warning", logType: LogType.Warning);
            Cucu.Log("Warning", logType: LogType.Warning);
        }

        private void CucuLoggerLog()
        {
            var logger = CucuLogger.Create().SetTag("logger");

            logger.SetType(LogType.Log).Log("Log");
            logger.SetType(LogType.Assert).Log("Assert");
            logger.SetType(LogType.Error).Log("Error");
            logger.SetType(LogType.Exception).Log("Exception");
            logger.SetType(LogType.Warning).Log("Warning");



            logger
                .SetTag("Loading")
                .SetType(LogType.Log)
                .SetArea(LogArea.Editor);

            var count = UnityEngine.Random.Range(8, 32);
            for (var i = 0; i < count; i++)
            {
                var val = Mathf.Clamp01(1f * i / (count - 1));

                var color = val.GetColorLerp(colors);

                var message = (int)(val * 100) + "%";
                message = $"{(message.Length < 3 ? "0" : "")}{message}";
                message = $"{(message.Length < 4 ? " " : "")}{message}";

                logger
                    .SetTag(color)
                    .Log(message);
            }
        }

        private void Error()
        {
            DivisionByZero();
        }

        private void DivisionByZero()
        {
            var x = 1;
            x = 1 / (x - 1);
        }
    }
}