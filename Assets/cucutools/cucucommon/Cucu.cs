using System;
using cucu.tools;
using UnityEngine;

namespace cucu
{
    class Cucu
    {
        public static readonly CucuLogger Logger;

        static Cucu()
        {
            Logger = CucuLogger.Create()
                .SetTag(null)
                .SetType(CucuLogger.LogType.Log)
                .SetArea(CucuLogger.LogArea.Application);
        }

        public static void Log(object message,
            string tag = null,
            Color? tagColor = null,
            CucuLogger.LogType? logType = null,
            CucuLogger.LogArea? logArea = null)
        {
            Logger.Log(message, tag, tagColor, logType, logArea);
        }

        public static string StringSetColor(string text, Color color)
        {
            return $"<color=\"#{Cucu.ColorToHex(color)}\">{text}</color>";
        }

        public static string ColorToHex(Color color) // TODO extenstion
        {
            var red = Convert.ToString((int)Mathf.Clamp(color.r * 255, 0, 255), 16);
            if (red.Length < 2) red = "0" + red;
            var green = Convert.ToString((int)Mathf.Clamp(color.g * 255, 0, 255), 16);
            if (green.Length < 2) green = "0" + green;
            var blue = Convert.ToString((int)Mathf.Clamp(color.b * 255, 0, 255), 16);
            if (blue.Length < 2) blue = "0" + blue;

            var result = red + green + blue;

            return result;
        }
    }
}