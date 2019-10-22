using cucu.tools;
using UnityEngine;

namespace cucu.example
{
    public class ExampleCucuLog : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) Do();
        }

        private void Do()
        {
            //Cucu.Log(null, null, null, null, null);
            //Debug.Log("I'm Debug.Log");

            var logger = CucuLogger.Create()
                .SetTag(Color.blue, "Loading")
                .SetType(CucuLogger.LogType.Log)
                .SetArea(CucuLogger.LogArea.Editor);

            Debug.LogError("ой-ой");

            logger.SetType(CucuLogger.LogType.Error).Log("i am simple logger");


            logger.SetType(CucuLogger.LogType.Log);
            logger.Log("Let's begin!", "Info");

            var colors = new[]
            {
                Color.red, Color.yellow.LerpTo(Color.red), Color.yellow, Color.green, Color.cyan,
                Color.blue, Color.magenta
            };

            var count = 16;
            for (var i = 0; i < count; i++)
            {
                float val = Mathf.Clamp01(1f * i / (count - 1));


                var color = val.GetColorLerp(colors);

                var message = (int) (val * 100) + "%";
                message = $"{(message.Length < 3 ? "0" : "")}{message}";
                message = $"{(message.Length < 4 ? " " : "")}{message}";

                logger.SetTag(color).Log(message);
            }

            ErrorFunc();
        }

        private void ErrorFunc()
        {
            Cucu.Log("I'm Cucu.Log", "Hi", logType: CucuLogger.LogType.Error);

            ErrorFunc2();
        }

        private void ErrorFunc2()
        {
            var x = 1;
            var ii = 1 / (x - 1);
        }
    }
}