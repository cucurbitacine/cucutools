using System;
using UnityEngine;

namespace CucuTools
{
    public abstract class CucuInjectorBehaviour : MonoBehaviour
    {
        private CucuArgumentManager ArgumentManager => CucuArgumentManager.Instance;
        
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