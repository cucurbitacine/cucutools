using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools
{
    public class CucuCollisionBehaviour : MonoBehaviour, ICucuCollision<CucuCollisionBehaviour>
    {
        #region Public

        public enum CollisionState
        {
            Enter,
            Stay,
            Exit,
        }

        public bool Active
        {
            get => _active;
            private set => _active = value;
        }

        public LayerMask LayerMask => _layerMask;

        #endregion

        #region Protected

        #endregion

        #region From editor

        [Header("Active state")] [SerializeField]
        private bool _active = true;

        [Header("Layer mask")] [SerializeField]
        private LayerMask _layerMask = new LayerMask {value = -1};

        #endregion

        #region Private

        private Collider _collider;
        private List<Action<Collision>> onEnterList = new List<Action<Collision>>();
        private List<Action<Collision>> onStayList = new List<Action<Collision>>();
        private List<Action<Collision>> onExitList = new List<Action<Collision>>();

        #endregion

        #region Hashing

        private List<Type> _lastHashList = new List<Type>();
        private CollisionState _lastHashState;
        private bool _isHashActual;

        #endregion

        public CucuCollisionBehaviour SetCollider(Collider collider)
        {
            _collider = collider;
            return this;
        }
        
        public CucuCollisionBehaviour SetLayerMask(LayerMask layerMask)
        {
            _layerMask = layerMask;
            return this;
        }

        public CucuCollisionBehaviour OnEnter(params Action<Collision>[] actions)
        {
            onEnterList.Clear();
            onEnterList.AddRange(actions);
            return this;
        }
        
        public CucuCollisionBehaviour OnStay(params Action<Collision>[] actions)
        {
            onStayList.Clear();
            onStayList.AddRange(actions);
            return this;
        }
        
        public CucuCollisionBehaviour OnExit(params Action<Collision>[] actions)
        {
            onExitList.Clear();
            onExitList.AddRange(actions);
            return this;
        }

        public CucuCollisionBehaviour SetActive(bool value = true)
        {
            Active = value;
            return this;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!Active) return; 

            if (!other.gameObject.IsValidLayer(LayerMask)) return;
            
            foreach (var enter in onEnterList)
                enter?.Invoke(other);
        }
        
        private void OnCollisionStay(Collision other)
        {
            if (!Active) return; 

            if (!other.gameObject.IsValidLayer(LayerMask)) return;
            
            foreach (var stay in onStayList)
                stay?.Invoke(other);
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (!Active) return; 

            if (!other.gameObject.IsValidLayer(LayerMask)) return;
            
            foreach (var exit in onExitList)
                exit?.Invoke(other);
        }
        
    }

    public interface ICucuCollision<TImpl> where TImpl : ICucuCollision<TImpl>
    {
        bool Active { get; }
        
        LayerMask LayerMask { get; }
        
        TImpl SetActive(bool value = true);
        
        TImpl SetLayerMask(LayerMask layerMask);

        TImpl OnEnter(params Action<Collision>[] actions);
        
        TImpl OnStay(params Action<Collision>[] actions);
        
        TImpl OnExit(params Action<Collision>[] actions);
    }
}