using System;
using UnityEngine;

namespace CucuTools.ArgumentInjector
{
    /// <summary>
    /// Some argument
    /// </summary>
    [Serializable]
    public abstract class CucuArg
    {
        public bool IsValid => isValid;
        [SerializeField] protected bool isValid = true;
    }
}