using System;
using UnityEngine;

namespace CucuTools.ArgumentInjector
{
    [Serializable]
    public abstract class CucuArg
    {
        public bool IsValid => isValid;
        [SerializeField] protected bool isValid = true;
    }
}