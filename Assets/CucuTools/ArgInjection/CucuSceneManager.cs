using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CucuTools
{
    public static class CucuSceneManager
    {
        private static CucuArgumentManager ArgumentManager => CucuArgumentManager.Instance;

        private static AsyncOperation LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(name, mode);
        }

        public static AsyncOperation LoadSceneAsync(string name, LoadSceneMode mode, params object[] args)
        {
            ArgumentManager.SetArguments(args);

            return LoadSceneAsync(name, mode);
        }

        public static AsyncOperation LoadSingleSceneAsync(string name, params object[] args)
        {
            return LoadSceneAsync(name, LoadSceneMode.Single, args);
        }

        public static AsyncOperation LoadAdditiveSceneAsync(string name, params object[] args)
        {
            return LoadSceneAsync(name, LoadSceneMode.Additive, args);
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class InjectArgAttribute : Attribute
    {
    }

    [Serializable]
    public abstract class CucuArg
    {
        public bool IsValid => isValid;
        [SerializeField] protected bool isValid;

        protected CucuArg()
        {
            isValid = true;
        }
    }
}