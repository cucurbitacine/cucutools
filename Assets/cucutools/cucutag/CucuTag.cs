using UnityEditor;
using UnityEngine;

namespace cucu.tools
{
    public class CucuTag : MonoBehaviour
    {
        public CucuEvent OnChanged => _onChanged ?? (_onChanged = new CucuEvent());

        private CucuEvent _onChanged;

        private GUID? _guid;

        [SerializeField] private string _key = "";

        public GUID Guid => (GUID) (_guid ?? (_guid = GUID.Generate()));

        public string Key
        {
            get => _key;
            set
            {
                var val = value?.ToLower();

                var newKey = val ?? "";

                if (_key?.Equals(val ?? $"#{_key}") ?? false) return;

                var isFirstTime = _key == null;

                var oldKey = _key ?? "";
                _key = newKey;

                OnChanged.Invoke();
            }
        }

        private void Awake()
        {
            Key = _key;
            OnChanged.AddListener(() => { CucuTagManager.Instance.EditTag(this); });
        }

        public void OnEnable()
        {
            CucuTagManager.Instance.AddTag(this);
        }

        public void OnDisable()
        {
            CucuTagManager.Instance.RemoveTag(this);
        }
    }
}