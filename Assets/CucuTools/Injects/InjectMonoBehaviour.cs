using System;
using UnityEngine;

namespace CucuTools.Injects
{
    /// <summary>
    /// Simple MonoBehaviour but with Inject on awake
    /// </summary>
    public abstract class InjectMonoBehaviour : CucuBehaviour
    {
        private CucuArgumentManager CucuArgumentManager => CucuArgumentManager.Singleton;

        protected abstract void OnAwake();

        private void Inject()
        {
            try
            {
                Injector.Inject(this, CucuArgumentManager.GetArgs());
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