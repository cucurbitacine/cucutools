using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace cucu.tools
{
    public class CucuTrigger : MonoBehaviour
    {
        public enum TriggerState
        {
            Enter,
            Stay,
            Exit,
        }

        public CucuEvent OnUpdateListOnEnter { get; private set; } = new CucuEvent();
        public CucuEvent OnUpdateListOnStay { get; private set; } = new CucuEvent();
        public CucuEvent OnUpdateListOnExit { get; private set; } = new CucuEvent();
        public CucuEvent OnUpdateListAny { get; private set; } = new CucuEvent();

        public IReadOnlyList<Type> RegTypesOnEnter => GetDictionaryByState(TriggerState.Enter).Keys.ToList();

        public IReadOnlyList<Type> RegTypesOnStay => GetDictionaryByState(TriggerState.Stay).Keys.ToList();

        public IReadOnlyList<Type> RegTypesOnExit => GetDictionaryByState(TriggerState.Exit).Keys.ToList();

        [Header("List of registered types from editor")]
        [SerializeField] private RegCompUnit[] _registeredComponentsOnEnter;
        [SerializeField] private RegCompUnit[] _registeredComponentsOnStay;
        [SerializeField] private RegCompUnit[] _registeredComponentsOnExit;

        [Space]

        [Header("List of ALL registered types")]
        [SerializeField] private RegTypeUnit[] _registeredTypesOnEnter;
        [SerializeField] private RegTypeUnit[] _registeredTypesOnStay;
        [SerializeField] private RegTypeUnit[] _registeredTypesOnExit;

        private Collider _collider;
        private Rigidbody _rigidbody;
        private Dictionary<TriggerState, Dictionary<Type, CucuObjectEvent>> _registeredTypes;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;

            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null) _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;

#if UNITY_EDITOR
            OnUpdateListOnEnter.AddListener(() =>
            {
                _registeredTypesOnEnter =
                    RegTypesOnEnter.Select(rtl => new RegTypeUnit {Type = rtl.ToString()}).ToArray();
            });

            OnUpdateListOnStay.AddListener(() =>
            {
                _registeredTypesOnStay =
                    RegTypesOnStay.Select(rtl => new RegTypeUnit {Type = rtl.ToString()}).ToArray();
            });

            OnUpdateListOnExit.AddListener(() =>
            {
                _registeredTypesOnExit =
                    RegTypesOnExit.Select(rtl => new RegTypeUnit {Type = rtl.ToString()}).ToArray();
            });
#endif

            OnUpdateListOnEnter.AddListener(OnUpdateListAny.Invoke);
            OnUpdateListOnStay.AddListener(OnUpdateListAny.Invoke);
            OnUpdateListOnExit.AddListener(OnUpdateListAny.Invoke);

            RegisterComponentsFromEditor();
        }

        public CucuObjectEvent RegisterComponent<TComponent>(TriggerState state = TriggerState.Enter)
            where TComponent : Component =>
            RegisterType(typeof(TComponent), state);

        public void RemoveComponent<TComponent>(TriggerState state = TriggerState.Enter)
            where TComponent : Component =>
            RemoveType(typeof(TComponent), state);

        public void RemoveComponentFromAll<TComponent>()
            where TComponent : Component =>
            RemoveTypeFromAll(typeof(TComponent));

        public CucuObjectEvent RegisterType(Type type, TriggerState state = TriggerState.Enter)
        {
            if (type == null) throw new NullReferenceException($"Type is null");

            var regTypes = GetDictionaryByState(state);

            if (!regTypes.ContainsKey(type))
            {
                regTypes.Add(type, new CucuObjectEvent());
                UpdateListInvoke(state);
            }

            return regTypes[type];
        }

        public void RemoveType(Type type, TriggerState state = TriggerState.Enter)
        {
            var regTypes = GetDictionaryByState(state);

            if (regTypes.TryGetValue(type, out var cucuEvent))
            {
                cucuEvent.RemoveAllListeners();
                if (regTypes.Remove(type)) UpdateListInvoke(state);
            }
        }

        public void RemoveTypeFromAll(Type type)
        {
            RemoveType(type, TriggerState.Enter);
            RemoveType(type, TriggerState.Stay);
            RemoveType(type, TriggerState.Exit);
        }

        private Dictionary<Type, CucuObjectEvent> GetDictionaryByState(TriggerState state)
        {
            if (_registeredTypes == null)
                _registeredTypes = new Dictionary<TriggerState, Dictionary<Type, CucuObjectEvent>>();

            if (!_registeredTypes.ContainsKey(state))
                _registeredTypes.Add(state, new Dictionary<Type, CucuObjectEvent>());

            return _registeredTypes[state];
        }

        private void UpdateListInvoke(TriggerState state)
        {
            switch (state)
            {
                case TriggerState.Enter:
                    OnUpdateListOnEnter.Invoke();
                    break;
                case TriggerState.Stay:
                    OnUpdateListOnStay.Invoke();
                    break;
                case TriggerState.Exit:
                    OnUpdateListOnExit.Invoke();
                    break;
            }
        }

        private void RegisterComponentsFromEditor()
        {
            foreach (var component in _registeredComponentsOnEnter)
                if(component.Component != null) RegisterType(component.Component.GetType(), TriggerState.Enter).AddListener(component.Event.Invoke);
            foreach (var component in _registeredComponentsOnStay)
                if (component.Component != null) RegisterType(component.Component.GetType(), TriggerState.Stay).AddListener(component.Event.Invoke);
            foreach (var component in _registeredComponentsOnExit)
                if (component.Component != null) RegisterType(component.Component.GetType(), TriggerState.Exit).AddListener(component.Event.Invoke);
        }

        private void OnTrigger(Collider other, TriggerState state)
        {
            var regTypes = GetDictionaryByState(state);

            foreach (var component in other.gameObject.GetComponents<Component>())
                if (regTypes.TryGetValue(component.GetType(), out var cucuEvent))
                    cucuEvent.Invoke(component);
        }

        private void OnTriggerEnter(Collider other) => OnTrigger(other, TriggerState.Enter);

        private void OnTriggerStay(Collider other) => OnTrigger(other, TriggerState.Stay);

        private void OnTriggerExit(Collider other) => OnTrigger(other, TriggerState.Exit);

        [Serializable]
        private struct RegCompUnit
        {
            public Component Component;

            public CucuObjectEvent Event;
        }

        [Serializable]
        private struct RegTypeUnit
        {
            public string Type;
        }
    }
}