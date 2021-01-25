using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc cref="ICucuCollision" />
    [RequireComponent(typeof(Collider))]
    public class CucuTriggerBehaviour : MonoBehaviour, ICucuCollision<CucuTriggerBehaviour, Collider>
    {
        #region Public

        /// <inheritdoc />
        public bool IsEnabled
        {
            get => isEnabled;
            set => SetEnable(value);
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
        [SerializeField] private bool isEnabled = true;

        [Header("Layer mask")]
        [SerializeField] private LayerMask layerMask = new LayerMask {value = 0};
        
        #endregion
        
        #region Private
        
        private readonly List<Action<Collider>> _onEnterList = new List<Action<Collider>>();
        private readonly List<Action<Collider>> _onStayList = new List<Action<Collider>>();
        private readonly List<Action<Collider>> _onExitList = new List<Action<Collider>>();

        private Collider _collider = null;
        
        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            Validation();
        }
        
        private void OnTriggerEnter(Collider coll)
        {
            if (!IsEnabled) return; 

            if (!coll.gameObject.IsValidLayer(LayerMask)) return;
            
            foreach (var enter in _onEnterList)
                enter?.Invoke(coll);
        }
        
        private void OnTriggerStay(Collider coll)
        {
            if (!IsEnabled) return; 

            if (!coll.gameObject.IsValidLayer(LayerMask)) return;
            
            foreach (var stay in _onStayList)
                stay?.Invoke(coll);
        }
        
        private void OnTriggerExit(Collider coll)
        {
            if (!IsEnabled) return; 

            if (!coll.gameObject.IsValidLayer(LayerMask)) return;
            
            foreach (var exit in _onExitList)
                exit?.Invoke(coll);
        }

        #endregion

        #region ICucuCollision

        /// <inheritdoc />
        public CucuTriggerBehaviour SetEnable(bool value = true)
        {
            IsEnabled = value;
            return this;
        }
        
        /// <inheritdoc />
        public CucuTriggerBehaviour SetLayerMask(LayerMask newLayerMask)
        {
            layerMask = newLayerMask;
            return this;
        }

        /// <inheritdoc />
        public CucuTriggerBehaviour OnEnter(params Action<Collider>[] actions)
        {
            _onEnterList.Clear();
            _onEnterList.AddRange(actions);
            return this;
        }
        
        /// <inheritdoc />
        public CucuTriggerBehaviour OnStay(params Action<Collider>[] actions)
        {
            _onStayList.Clear();
            _onStayList.AddRange(actions);
            return this;
        }
        
        /// <inheritdoc />
        public CucuTriggerBehaviour OnExit(params Action<Collider>[] actions)
        {
            _onExitList.Clear();
            _onExitList.AddRange(actions);
            return this;
        }

        /// <inheritdoc />
        public bool IsValid()
        {
            return _collider != null && _collider.isTrigger;
        }

        /// <inheritdoc />
        public bool Validation()
        {
            if (IsValid()) return true;

            _collider = GetComponent<Collider>();
            if (_collider == null) return false;
            
            _collider.isTrigger = true;
            return true;
        }

        #endregion
    }
}