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

        public static void SetColorUsePropertyBlock(this Renderer renderer, Color color, string propertyName = "_Color")
        {
            var matPropBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(matPropBlock);
            matPropBlock.SetColor(propertyName, color);
            renderer.SetPropertyBlock(matPropBlock);
        }
        
        public static void SetEmissionColorUsePropertyBlock(this Renderer renderer, Color color)
        {
            renderer.SetColorUsePropertyBlock(color, "_EmissionColor");
        }
    }
}