using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    [RequireComponent(typeof(Collider))]
    public class CucuCollisionBehaviour : MonoBehaviour, ICucuCollision<CucuCollisionBehaviour, Collision>
    {
        #region Public
        
        /// <inheritdoc />
        public bool Active
        {
            get => active;
            set => SetActive(value);
        }

        /// <inheritdoc />
        public LayerMask LayerMask
        {
            get => layerMask;
            set => SetLayerMask(value);
        }

        #endregion

        #region Protected

        #endregion

        #region From editor

        [Header("Active state")]
        [SerializeField] private bool active = true;

        [Header("Layer mask")]
        [SerializeField] private LayerMask layerMask = new LayerMask {value = 0};

        #endregion

        #region Private
        
        private readonly List<Action<Collision>> _onEnterList = new List<Action<Collision>>();
        private readonly List<Action<Collision>> _onStayList = new List<Action<Collision>>();
        private readonly List<Action<Collision>> _onExitList = new List<Action<Collision>>();

        private Collider _collider = null;
        
        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            Validation();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!Active) return; 

            if (!other.gameObject.IsValidLayer(LayerMask)) return;
            
            foreach (var enter in _onEnterList)
                enter?.Invoke(other);
        }
        
        private void OnCollisionStay(Collision other)
        {
            if (!Active) return; 

            if (!other.gameObject.IsValidLayer(LayerMask)) return;
            
            foreach (var stay in _onStayList)
                stay?.Invoke(other);
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (!Active) return; 

            if (!other.gameObject.IsValidLayer(LayerMask)) return;
            
            foreach (var exit in _onExitList)
                exit?.Invoke(other);
        }

        #endregion

        #region ICucuCollision

        /// <inheritdoc />
        public CucuCollisionBehaviour SetActive(bool value = true)
        {
            Active = value;
            return this;
        }
        
        /// <inheritdoc />
        public CucuCollisionBehaviour SetLayerMask(LayerMask newLayerMask)
        {
            layerMask = newLayerMask;
            return this;
        }

        /// <inheritdoc />
        public CucuCollisionBehaviour OnEnter(params Action<Collision>[] actions)
        {
            _onEnterList.Clear();
            _onEnterList.AddRange(actions);
            return this;
        }
        
        /// <inheritdoc />
        public CucuCollisionBehaviour OnStay(params Action<Collision>[] actions)
        {
            _onStayList.Clear();
            _onStayList.AddRange(actions);
            return this;
        }
        
        /// <inheritdoc />
        public CucuCollisionBehaviour OnExit(params Action<Collision>[] actions)
        {
            _onExitList.Clear();
            _onExitList.AddRange(actions);
            return this;
        }

        /// <inheritdoc />
        public bool IsValid()
        {
            return _collider != null && !_collider.isTrigger;
        }

        /// <inheritdoc />
        public bool Validation()
        {
            if (IsValid()) return true;

            _collider = GetComponent<Collider>();
            if (_collider == null) return false;
            
            _collider.isTrigger = false;
            return true;
        }

        #endregion
    }
}