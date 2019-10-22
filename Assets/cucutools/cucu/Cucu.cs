using cucu.tools;
using UnityEngine;

namespace cucu
{
    internal class Cucu
    {
        public static readonly CucuLogger Logger;

        static Cucu()
        {
            Logger = CucuLogger.Create()
                .SetTag(null)
                .SetType(CucuLogger.LogType.Log)
                .SetArea(CucuLogger.LogArea.Application);
        }

        private Cucu()
        {
        }

        public static void Log(object message,
            string tag = null,
            Color? tagColor = null,
            CucuLogger.LogType? logType = null,
            CucuLogger.LogArea? logArea = null)
        {
            Logger.Log(message, tag, tagColor, logType, logArea);
        }
    }
}