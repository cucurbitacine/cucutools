using System;
using CucuTools.ArgumentInjector;
using UnityEngine;

namespace CucuTools.Common
{
    /// <summary>
    /// Simple MonoBehaviour but with Inject on awake
    /// </summary>
    public abstract class CucuMonoBehaviour : MonoBehaviour
    {
        private CucuArgumentManager ArgumentManager => CucuArgumentManager.Singleton;
        
        protected virtual void OnAwake()
        {
        }

        private void Inject()
        {
            try
            {
                CucuInjector.Inject(this, ArgumentManager.GetArgs());
            }
            catch (Exception exc)
            {
                Debug.LogError($"Injection failed :: {exc}");
            }
        }
        
        protected virtual void Awake()
        {
            Inject();
            
            OnAwake();
        }
    }
}