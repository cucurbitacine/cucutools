using CucuTools.ArgumentInjector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CucuTools.Common
{
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

        #endregion
        
        private static AsyncOperation LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return SceneManager.LoadSceneAsync(name, mode);
        }

        private static void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(name, mode);
        }
    }
}