using Cucu.Log;
using UnityEngine;

namespace Cucu.Common
{
    internal class Cucu
    {
        public static readonly CucuLogger Logger;

        public static readonly Color[] Rainbow = new[] // TODO in "cucu colors"
        {
            Color.red,
            Color.red.LerpTo(Color.yellow),
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            Color.magenta
        };

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