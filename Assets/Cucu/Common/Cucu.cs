using Cucu.Log;
using UnityEngine;

namespace Cucu.Common
{
    internal class Cucu
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
    }
}