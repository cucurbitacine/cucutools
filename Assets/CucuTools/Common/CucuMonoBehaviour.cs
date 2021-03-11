using System;
using CucuTools.ArgumentInjector;
using UnityEngine;

namespace CucuTools.Common
{
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
        
        private void Awake()
        {
            Inject();
            
            OnAwake();
        }
    }
}