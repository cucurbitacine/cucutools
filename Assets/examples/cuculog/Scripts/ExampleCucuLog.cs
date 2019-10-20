using cucu.tools;
using UnityEngine;

namespace cucu.example
{
    public class ExampleCucuLog : MonoBehaviour
    {
        private void Start()
        {
            Cucu.Log(null, null, null, null, null);
            Cucu.Log("I'm Justin", "Hi");
            
            var logger = CucuLogger.Create()
                .SetTag(Color.blue, "Loading")
                .SetType(CucuLogger.LogType.Log)
                .SetArea(CucuLogger.LogArea.Editor);
            
            logger.Log($"Let's begin!", "Info");
            var count = 8;
            for (var i = 0; i < count; i++)
            {
                var val = Mathf.Clamp01(1f * i / (count - 1));
                var newVal = Mathf.Clamp01(2 * (val < 0.5f ? val : val - 0.5f));

                var color = val < 0.5f
                    ? Color.Lerp(Color.red, Color.yellow, newVal)
                    : Color.Lerp(Color.yellow, Color.green, newVal);

                var message = ((int) (val * 100)).ToString() + "%";
                message = $"{(message.Length < 3 ? "0" : "")}{message}";
                message = $"{(message.Length < 4 ? " " : "")}{message}";

                logger.SetTag(color).Log(message);
            }
        }
    }
}