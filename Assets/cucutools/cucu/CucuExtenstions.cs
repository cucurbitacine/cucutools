using System;
using UnityEngine;

namespace cucu
{
    public static class CucuExtenstions
    {
        public static string SetColor(this object obj, Color color)
        {
            return $"<color=#{color.GetHex()}>{obj}</color>";
        }

        public static string GetHex(this Color color)
        {
            var result = "";
            for (var i = 0; i < 3; i++)
            {
                var hex = Convert.ToString((int) Mathf.Clamp(color[i] * 255, 0, 255), 16);
                if (hex.Length < 2) hex = "0" + hex;
                result += hex;
            }

            return result;
        }
    }
}