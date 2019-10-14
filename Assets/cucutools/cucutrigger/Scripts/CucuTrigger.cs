using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace cucu.tools
{
    public class CucuTrigger : MonoBehaviour
    {
        #region Public

        public enum TriggerState
        {
            Enter,
            Stay,
            Exit,
        }

        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        public LayerMask LayerMask => _layerMask;

        public CucuEvent OnUpdateList { get; private set; } = new CucuEvent();

        #endregion

        #region Protected



        #endregion

        #region From editor

        [Header("Active state")]
        [SerializeField] private bool _active = true;

        [Space]

        [Header("Layer mask")]
        [SerializeField] private LayerMask _layerMask = new LayerMask { value = -1 };

        [Space]

        [Header("List of registered types from editor")]
        [SerializeField] private RegCompUnit[] _registeredComponentsOnEnter;
        [SerializeField] private RegCompUnit[] _registeredComponentsOnStay;
        [SerializeField] private RegCompUnit[] _registeredComponentsOnExit;

        [Space]

        [Header("List of ALL registered types")]
        [SerializeField] private RegTypeUnit[] _registeredTypesOnEnter;
        [SerializeField] private RegTypeUnit[] _registeredTypesOnStay;
        [SerializeField] private RegTypeUnit[] _registeredTypesOnExit;

        #endregion

        #region Private

        private Collider _collider;
        private Rigidbody _rigidbody;
        private Dictionary<TriggerState, Dictionary<Type, CucuObjectEvent>> _registeredTypes;

        #endregion

        #region Hashing

        private List<Type> _lastHashList = new List<Type>();
        private TriggerState _lastHashState;
        private bool _isHashActual;

        #endregion

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;

            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null) _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.isKinematic = true;

#if UNITY_EDITOR
            OnUpdateList.AddListener(() =>
            {
                _registeredTypesOnEnter =
                    GetListTypesByState(TriggerState.Enter).Select(rtl => new RegTypeUnit {Type = rtl.ToString()}).ToArray();

                _registeredTypesOnStay =
                    GetListTypesByState(TriggerState.Stay).Select(rtl => new RegTypeUnit { Type = rtl.ToString() }).ToArray();

                _registeredTypesOnExit =
                    GetListTypesByState(TriggerState.Exit).Select(rtl => new RegTypeUnit { Type = rtl.ToString() }).ToArray();
            });
#endif

            OnUpdateList.AddListener(() => _isHashActual = false);

            RegisterComponentsFromEditor();
        }

        #region Registry

        public CucuObjectEvent RegisterComponent<TComponent>(TriggerState state = TriggerState.Enter) where TComponent : Component =>
            RegisterType(typeof(TComponent), state);

        public void RemoveComponent<TComponent>(TriggerState state = TriggerState.Enter) where TComponent : Component =>
            RemoveType(typeof(TComponent), state);

        public void RemoveComponentFromAll<TComponent>() where TComponent : Component =>
            RemoveTypeFromAll(typeof(TComponent));

        public CucuObjectEvent RegisterType(Type type, TriggerState state = TriggerState.Enter)
        {
            if (type == null) throw new NullReferenceException($"Type is null");

            var regTypes = GetDictionaryTypesByState(state);

            if (!regTypes.ContainsKey(type))
            {
                regTypes.Add(type, new CucuObjectEvent());
                OnUpdateList.Invoke();
            }

            return regTypes[type];
        }

        public void RemoveType(Type type, TriggerState state = TriggerState.Enter)
        {
            var regTypes = GetDictionaryTypesByState(state);

            if (regTypes.TryGetValue(type, out var cucuEvent))
            {
                cucuEvent.RemoveAllListeners();
                if (regTypes.Remove(type)) OnUpdateList.Invoke();
            }
        }

        public void RemoveTypeFromAll(Type type)
        {
            RemoveType(type, TriggerState.Enter);
            RemoveType(type, TriggerState.Stay);
            RemoveType(type, TriggerState.Exit);
        }

        #endregion

        #region Public methods

        public List<Type> GetListTypesByState(TriggerState state)
        {
            if (_isHashActual && _lastHashState == state) return _lastHashList;

            _lastHashList.Clear();
            _lastHashList = GetDictionaryTypesByState(TriggerState.Enter).Keys.ToList();

            _lastHashState = state;
            _isHashActual = true;

            return _lastHashList;
        }

        #endregion

        #region Protected methods

        protected Dictionary<Type, CucuObjectEvent> GetDictionaryTypesByState(TriggerState state)
        {
            if (_registeredTypes == null)
                _registeredTypes = new Dictionary<TriggerState, Dictionary<Type, CucuObjectEvent>>();

            if (!_registeredTypes.ContainsKey(state))
                _registeredTypes.Add(state, new Dictionary<Type, CucuObjectEvent>());

            return _registeredTypes[state];
        }

        protected bool IsValidObjectLayer(GameObject gObj)
        {
            return (_layerMask.value & (1 << gObj.layer)) > 0;
        }

        #endregion

        #region Private methods

        private void RegisterComponentsFromEditor()
        {
            foreach (var component in _registeredComponentsOnEnter)
                if (component.Component != null)
                    RegisterType(component.Component.GetType(), TriggerState.Enter).AddListener(component.Event.Invoke);

            foreach (var component in _registeredComponentsOnStay)
                if (component.Component != null)
                    RegisterType(component.Component.GetType(), TriggerState.Stay).AddListener(component.Event.Invoke);

            foreach (var component in _registeredComponentsOnExit)
                if (component.Component != null)
                    RegisterType(component.Component.GetType(), TriggerState.Exit).AddListener(component.Event.Invoke);
        }

        #endregion

        #region Triggers

        private void OnTrigger(Collider other, TriggerState state)
        {
            var gObj = other.gameObject;

            if (!IsValidObjectLayer(gObj)) return;

            var regTypes = GetDictionaryTypesByState(state);

            foreach (var component in gObj.GetComponents<Component>())
            {
                if (component == null) continue;

                if (regTypes.TryGetValue(component.GetType(), out var cucuEvent))
                    cucuEvent.Invoke(component);

            }
        }

        private void OnTriggerEnter(Collider other) => OnTrigger(other, TriggerState.Enter);

        private void OnTriggerStay(Collider other) => OnTrigger(other, TriggerState.Stay);

        private void OnTriggerExit(Collider other) => OnTrigger(other, TriggerState.Exit);

        #endregion

        #region Structs

        [Serializable]
        protected struct RegCompUnit
        {
            public Component Component;

            public CucuObjectEvent Event;
        }

        [Serializable]
        protected struct RegTypeUnit
        {
            public string Type;
        }

        #endregion
    }
}