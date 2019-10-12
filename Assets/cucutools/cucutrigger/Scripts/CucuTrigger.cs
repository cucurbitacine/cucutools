using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace cucu.tools
{
    public class CucuTrigger : MonoBehaviour
    {
        private Collider _collider;
        private Rigidbody _rigidbody;

        [SerializeField] private Component[] _registerComponents;

        private Dictionary<Type, CucuObjectEvent> _registeredTypes;

        public IReadOnlyList<Type> RegisteredTypesList => _registeredTypes.Keys.ToList();
        [SerializeField] private string[] _typeNames;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;

            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null) _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;

            _registeredTypes = new Dictionary<Type, CucuObjectEvent>();
            foreach (var comp in _registerComponents) RegisterComponent(comp.GetType());
        }

        public CucuObjectEvent RegisterComponent<TComponent>()
            where TComponent : Component =>
            RegisterComponent(typeof(TComponent));

        public void RemoveComponent<TComponent>()
            where TComponent : Component =>
            RemoveComponent(typeof(TComponent));

        public CucuObjectEvent RegisterComponent(Type type)
        {
            if (_registeredTypes == null) _registeredTypes = new Dictionary<Type, CucuObjectEvent>();
            if (!_registeredTypes.ContainsKey(type))
            {
                _registeredTypes.Add(type, new CucuObjectEvent());
                UpdateViewList();
            }
            return _registeredTypes[type];
        }
        
        public void RemoveComponent(Type type)
        {
            if (_registeredTypes.TryGetValue(type, out var cucuEvent))
            {
                cucuEvent.RemoveAllListeners();
                if (_registeredTypes.Remove(type))
                    UpdateViewList();
            }
        }

        private void UpdateViewList()
        {
            _typeNames = RegisteredTypesList
                .Select(rtl => rtl.ToString())
                .ToArray();
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach(var component in other.gameObject.GetComponents<Component>())
                if (_registeredTypes.TryGetValue(component.GetType(), out var cucuEvent))
                    cucuEvent.Invoke(component);                    
        }
    }
}