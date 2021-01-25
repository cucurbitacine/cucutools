using System;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [Serializable]
    public class CucuTrigger : ICucuCollision<CucuTrigger, Collider>
    {
        /// <inheritdoc />
        public bool IsEnabled
        {
            get => triggerBehaviour != null && triggerBehaviour.IsEnabled;
            set => SetEnable(value);
        }

        /// <inheritdoc />
        public LayerMask LayerMask
        {
            get => triggerBehaviour != null ? triggerBehaviour.LayerMask : new LayerMask() {value = -1};
            set => SetLayerMask(value);
        }

        private CucuTriggerBehaviour triggerBehaviour;

        public CucuTrigger(CucuTriggerBehaviour triggerBehaviour)
        {
            this.triggerBehaviour = triggerBehaviour;
        }

        public CucuTrigger(Collider collider) : this(collider.gameObject.AddComponent<CucuTriggerBehaviour>())
        {
        }

        /// <inheritdoc />
        public CucuTrigger SetEnable(bool value)
        {
            triggerBehaviour.SetEnable(value);
            return this;
        }

        /// <inheritdoc />
        public CucuTrigger SetLayerMask(LayerMask newLayerMask)
        {
            triggerBehaviour.SetLayerMask(newLayerMask);
            return this;
        }

        /// <inheritdoc />
        public CucuTrigger OnEnter(params Action<Collider>[] actions)
        {
            triggerBehaviour.OnEnter(actions);
            return this;
        }

        /// <inheritdoc />
        public CucuTrigger OnStay(params Action<Collider>[] actions)
        {
            triggerBehaviour.OnStay(actions);
            return this;
        }

        /// <inheritdoc />
        public CucuTrigger OnExit(params Action<Collider>[] actions)
        {
            triggerBehaviour.OnExit(actions);
            return this;
        }

        /// <inheritdoc />
        public bool IsValid()
        {
            return triggerBehaviour != null && triggerBehaviour.IsValid();
        }

        /// <inheritdoc />
        public bool Validation()
        {
            return triggerBehaviour != null && triggerBehaviour.Validation();
        }
    }
}