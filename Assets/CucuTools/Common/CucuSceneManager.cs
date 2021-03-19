using System;
using System.Reflection;
using CucuTools.ArgumentInjector;
using CucuTools.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CucuTools.Common
{
    /// <summary>
    /// Cucu scene manager. Wrapper around <see cref="SceneManager"/> only with setting <see cref="CucuArg"/> by <see cref="CucuArgumentManager"/>
    /// </summary>
    public static class CucuSceneManager
    {
        private static CucuArgumentManager ArgumentManager => CucuArgumentManager.Singleton;

        #region Load async

        public static AsyncOperation LoadSceneAsync(string name, LoadSceneMode mode, object[] args)
        {
            ArgumentManager.SetArguments(args);

            return LoadSceneAsync(name, mode);
        }
        
        public static AsyncOperation LoadSingleSceneAsync(string name, object[] args)
        {
            return LoadSceneAsync(name, LoadSceneMode.Single, args);
        }
        
        public static AsyncOperation LoadAdditiveSceneAsync(string name, object[] args)
        {
            return LoadSceneAsync(name, LoadSceneMode.Additive, args);
        }

        public static AsyncOperation LoadSceneAsync<TController>(LoadSceneMode mode, object[] args)
            where TController : CucuSceneController
        {
            ArgumentManager.SetArguments(args);

            if (TryGetSceneName<TController>(out var name, out var msg))
                return LoadSceneAsync(name, mode);
            else
                throw new Exception($"Load scene of \"{typeof(TController).Name}\" was failed :: {msg}");
        }
        
        public static AsyncOperation LoadSingleSceneAsync<TController>(object[] args)
            where TController : CucuSceneController
        {
            return LoadSceneAsync<TController>(LoadSceneMode.Single, args);
        }
        
        public static AsyncOperation LoadAdditiveSceneAsync<TController>(object[] args)
            where TController : CucuSceneController
        {
            return LoadSceneAsync<TController>(LoadSceneMode.Additive, args);
        }
        
        #endregion

        #region Load

        public static void LoadScene(string name, LoadSceneMode mode, object[] args)
        {
            ArgumentManager.SetArguments(args);

            LoadScene(name, mode);
        }
        public static void LoadSingleScene(string name, object[] args)
        {
            LoadScene(name, LoadSceneMode.Single, args);
        }
        
        public static void LoadAdditiveScene(string name, object[] args)
        {
            LoadScene(name, LoadSceneMode.Additive, args);
        }

        public static void LoadScene<TController>(LoadSceneMode mode, object[] args)
            where TController : CucuSceneController
        {
            ArgumentManager.SetArguments(args);

            if (TryGetSceneName<TController>(out var name, out var msg))
                LoadScene(name, mode);
            else
                throw new Exception($"Load scene of \"{nameof(TController)}\" was failed :: {msg}");
        }
        
        public static void LoadSingleScene<TController>(object[] args)
            where TController : CucuSceneController
        {
            LoadScene<TController>(LoadSceneMode.Single, args);
        }
        
        public static void LoadAdditiveScene<TController>(object[] args)
            where TController : CucuSceneController
        {
            LoadScene<TController>(LoadSceneMode.Additive, args);
        }
        
        #endregion

        /// <summary>
        /// Async load scene <param name="name"></param> with mode <param name="mode"></param>
        /// </summary>
        /// <param name="name">Scene name</param>
        /// <param name="mode">Load mode</param>
        public static AsyncOperation LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(name, mode);
        }

        /// <summary>
        /// Load scene <param name="name"></param> with mode <param name="mode"></param>
        /// </summary>
        /// <param name="name">Scene name</param>
        /// <param name="mode">Load mode</param>
        public static void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(name, mode);
        }
        
        /// <summary>
        /// Trying get scene name from attribute of scene controller
        /// </summary>
        /// <param name="sceneName">Result scene name</param>
        /// <param name="msg">Message</param>
        /// <typeparam name="TController">Type of scene controller</typeparam>
        /// <returns>Success</returns>
        private static bool TryGetSceneName<TController>(out string sceneName, out string msg) where TController : CucuSceneController
        {
            sceneName = null;
            msg = "";
            
            var attribute = (SceneControllerAttribute) typeof(TController).GetCustomAttribute(typeof(SceneControllerAttribute));

            if (attribute == null)
            {
                msg = $"{nameof(SceneControllerAttribute)} was not found in custom attributes";
                return false;
            }
            
            sceneName = attribute.SceneName;

            if (string.IsNullOrWhiteSpace(sceneName))
            {
                msg = $"Scene name is null or white space";
                return false;
            }
            
            return true;
        }
    }
}