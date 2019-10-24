using UnityEditor;
using UnityEngine;

namespace cucu.tools
{
    public class CucuTag : MonoBehaviour
    {
        public CucuEvent OnChanged => _onChanged ?? (_onChanged = new CucuEvent());

        private CucuEvent _onChanged;

        private GUID? _guid;
#if UNITY_EDITOR
        [SerializeField] private string _keyFromEditor = "";
#endif
        private bool _online;
        private string _key;

        public bool IsOnline => _online;
        public bool IsAutoRegister => _autoRegister;
        [SerializeField] private bool _autoRegister;

        public string Key => _key;

        public GUID Guid => (GUID) (_guid ?? (_guid = GUID.Generate()));
        
        private void Awake()
        {
            _key = "";
#if UNITY_EDITOR
            _key = _keyFromEditor;
#endif
            OnChanged.AddListener(() => CucuTagManager.Instance.UpdateTag(this));
            if (IsAutoRegister) RegisterAs(Key);
        }
        
        public void Update()
        {
#if UNITY_EDITOR
            if (!_key.Equals(_keyFromEditor))
            {
                SetKey(_keyFromEditor);
            }
#endif
        }

        [ContextMenu(nameof(Register))]
        public bool Register()
        {
            if (!CucuTagManager.Instance.Register(this)) return false;
            return SwitchOn();
        }

        public bool RegisterAs(string key)
        {
            SetKey(key);
            return Register();
        }

        [ContextMenu(nameof(Unregistering))]
        public bool Unregistering()
        {
            if (!CucuTagManager.Instance.Unregistering(this)) return false;
            return true;
        }

        private bool SwitchOn()
        {
            if (!CucuTagManager.Instance.LogIn(this)) return false;
            _online = true;
            OnChanged.Invoke();
            return true;
        }

        private bool SwitchOff()
        {
            if (!CucuTagManager.Instance.LogOut(this)) return false;
            _online = false;
            OnChanged.Invoke();
            return true;
        }

        private bool ToggleStatus(bool online)
        {
            return online ? SwitchOn() : SwitchOff();
        }

        public void SetKey(string key)
        {
            _key = key ?? "";
#if UNITY_EDITOR
            _keyFromEditor = _key;
#endif
            OnChanged.Invoke();
        }

        private void OnEnable()
        {
            ToggleStatus(true);
        }

        private void OnDisable()
        {
            ToggleStatus(false);
        }

        private void OnDestroy()
        {
            Unregistering();
        }
    }
}