using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CucuTools.Common
{
    public class Cucu
    {
        public const string MenuRoot = "CucuTools/";
        public const string MenuCreateRoot = "GameObject/" + Cucu.MenuRoot + "Create/";
        
        private Cucu()
        {
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
    
    public static class CucuExtensions
    {
        #region Random

        public static T GetRandom<T>(this IEnumerable<T> enumerable)
        {
            return GetRandom(enumerable.ToArray());
        }

        public static T GetRandom<T>(params T[] array)
        {
            return array == null ? default : array[Random.Range(0, array.Length)];
        }

        #endregion
        
        #region Interfaces

        public static bool IsInterface(this Component component, Type type)
        {
            return component?.GetType().GetInterfaces().Contains(type) ?? false;
        }
        
        public static bool TryGetInterface(this Component component, Type type, out object result)
        {
            result = null;
            if (!component.IsInterface(type)) return false;
            result = component;
            return true;
        }

        public static bool TryGetInterface(this GameObject gameObject, Type type, out object result)
        {
            result = gameObject
                .GetComponents<Component>()
                .FirstOrDefault(c => c.IsInterface(type));

            return result != null;
        }

        public static bool TryGetInterface(this Transform transform, Type type, out object result)
        {
            return transform.gameObject.TryGetInterface(type, out result);
        }

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

        #endregion

        #region PropertyBlock

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

        #endregion

        #region Layers

        public static bool IsValidLayer(this int value, LayerMask layerMask)
        {
            return Cucu.IsValidLayer(layerMask, value);
        }

        public static bool IsValidLayer(this GameObject gameObject, LayerMask layerMask)
        {
            return gameObject.layer.IsValidLayer(layerMask);
        }

        public static bool IsValidLayer(this Transform transform, LayerMask layerMask)
        {
            return transform.gameObject.IsValidLayer(layerMask);
        }

        public static bool IsValidLayer(this Component component, LayerMask layerMask)
        {
            return component.transform.IsValidLayer(layerMask);
        }

        public static bool TryGetLayerName(this int value, out string layerName)
        {
            return Cucu.TryGetLayerName(value, out layerName);
        }

        #endregion
    }
}