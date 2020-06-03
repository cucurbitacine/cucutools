using UnityEngine;

namespace CucuTools
{
    public class Cucu
    {
        public static readonly CucuLogger Logger;
        
        static Cucu()
        {
            Logger = CucuLogger.Create()
                .SetTag(null)
                .SetType(LogType.Log)
                .SetArea(LogArea.Application);
        }

        private Cucu()
        {
        }

        public static void Log(object message,
            string tag = null,
            Color? tagColor = null,
            LogType? logType = null,
            LogArea? logArea = null)
        {
            Logger.Log(message, tag, tagColor, logType, logArea);
        }
        
        public static bool IsValidLayer(LayerMask layerMask, int value)
        {
            return (layerMask.value & (1 << value)) > 0;
        }
        
        
        public static bool TryGetLayerName(int index, out string name)
        {
            name = null;

            try
            {
                name = LayerMask.LayerToName(index);
            }
            catch
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(name);
        }
    }
}