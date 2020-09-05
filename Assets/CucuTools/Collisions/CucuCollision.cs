using System;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [Serializable]
    public class CucuCollision : ICucuCollision<CucuCollision, Collision>
    {
        /// <inheritdoc />
        public bool Active
        {
            get => collisionBehaviour != null && collisionBehaviour.Active;
            set => SetActive(value);
        }

        /// <inheritdoc />
        public LayerMask LayerMask
        {
            get => collisionBehaviour != null ? collisionBehaviour.LayerMask : new LayerMask() {value = -1};
            set => SetLayerMask(value);
        }

        private CucuCollisionBehaviour collisionBehaviour;
        
        public CucuCollision(CucuCollisionBehaviour collisionBehaviour)
        {
            this.collisionBehaviour = collisionBehaviour;
        }
        
        public CucuCollision(Collider collider) : this(collider.gameObject.AddComponent<CucuCollisionBehaviour>())
        {
        }

        /// <inheritdoc />
        public CucuCollision SetActive(bool value = true)
        {
            if (collisionBehaviour != null) collisionBehaviour.SetActive(value);
            return this;
        }

        /// <inheritdoc />
        public CucuCollision SetLayerMask(LayerMask newLayerMask)
        {
            if (collisionBehaviour != null) collisionBehaviour.SetLayerMask(newLayerMask);
            return this;
        }

        /// <inheritdoc />
        public CucuCollision OnEnter(params Action<Collision>[] actions)
        {
            if (IsValid()) collisionBehaviour.OnEnter(actions);
            return this;
        }

        /// <inheritdoc />
        public CucuCollision OnStay(params Action<Collision>[] actions)
        {
            if (IsValid()) collisionBehaviour.OnStay(actions);
            return this;
        }

        /// <inheritdoc />
        public CucuCollision OnExit(params Action<Collision>[] actions)
        {
            if (IsValid()) collisionBehaviour.OnExit(actions);
            return this;
        }

        /// <inheritdoc />
        public bool IsValid()
        {
            return collisionBehaviour != null && collisionBehaviour.IsValid();
        }
        
        /// <inheritdoc />
        public bool Validation()
        {
            return collisionBehaviour != null && collisionBehaviour.Validation();
        }
    }
}