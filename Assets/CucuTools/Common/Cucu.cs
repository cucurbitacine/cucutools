using UnityEngine;

namespace CucuTools
{
    public class Cucu
    {
        public const string MenuRoot = "CucuTools/";
        
        private Cucu()
        {
        }

        public static void Log(object message)
        {
            CucuLogger.Global.Log(message);
        }
        
        public static void LogWarning(object message)
        {
            CucuLogger.Global.LogWarning(message);
        }
        
        public static void LogError(object message)
        {
            CucuLogger.Global.LogError(message);
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