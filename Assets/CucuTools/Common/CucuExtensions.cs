using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools.Common
{
    public static class CucuExtensions
    {
        public static T GetRandom<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable.ToArray();
            return array[Random.Range(0, array.Length)];
        }

        public static Color GetColorUsePropertyBlock(this Renderer renderer, string propertyName)
        {
            if (renderer == null) return Color.black;
            var matPropBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(matPropBlock);
            return matPropBlock.GetColor(propertyName);
        }
        
        public static void SetColorUsePropertyBlock(this Renderer renderer, string propertyName, Color color)
        {
            if (renderer == null) return;
            var matPropBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(matPropBlock);
            matPropBlock.SetColor(propertyName, color);
            renderer.SetPropertyBlock(matPropBlock);
        }
        
        public static float GetFloatUsePropertyBlock(this Renderer renderer, string propertyName)
        {
            if (renderer == null) return 0.0f;
            var matPropBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(matPropBlock);
            return matPropBlock.GetFloat(propertyName);
        }

        public static void SetFloatUsePropertyBlock(this Renderer renderer, string propertyName, float value)
        {
            if (renderer == null) return;
            var matPropBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(matPropBlock);
            matPropBlock.SetFloat(propertyName, value);
            renderer.SetPropertyBlock(matPropBlock);
        }
    }
}