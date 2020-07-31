using CucuTools;
using UnityEngine;

public class TestLog : MonoBehaviour
{
    public bool toggle;

    public string message;
    public string tag;
    public LogType logType;
    public LogArea logArea;
    
    public CucuLogger logger;
    
    private void OnValidate()
    {
        logger = CucuLogger.Create()
            .SetTag(tag)
            .SetType(logType)
            .SetArea(logArea);
        
        logger.Log(message);
    }
}
