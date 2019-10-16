using cucu.tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace cucu.example
{
    public class ExampleCucuLog : MonoBehaviour
    {
        private void Start()
        {
            CucuLog.Log("Message", "Tag", Color.cyan, CucuLog.LogType.Warning);

            var logger = CucuLog.Create()
                .SetTag(Color.blue, "Loading")
                .SetType(CucuLog.LogType.Log)
                .SetArea(CucuLog.LogArea.Editor);
            
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
                if (message.Length < 3) message = "0" + message; 
                if (message.Length < 4) message = " " + message; 

                logger.SetTag(color).Log(message);
            }
        }
    }
}