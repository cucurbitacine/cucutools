﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public static class CucuExtensions
    {
        public static bool IsInterface<T>(this Component component) where T : class
        {
            return component?.GetType().GetInterfaces().Contains(typeof(T)) ?? false;
        }

        public static bool TryGetInterface<T>(this Component component, out T result) where T : class
        {
            result = null;
            if (!component.IsInterface<T>()) return false;
            result = component as T;
            return true;
        }

        public static bool TryGetInterface<T>(this GameObject gameObject, out T result) where T : class
        {
            result = gameObject
                .GetComponents<Component>()
                .FirstOrDefault(c => c.IsInterface<T>()) as T;

            return result != null;
        }

        public static bool TryGetInterface<T>(this Transform transform, out T result) where T : class
        {
            return transform.gameObject.TryGetInterface(out result);
        }
        
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