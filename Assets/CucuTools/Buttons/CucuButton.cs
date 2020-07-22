using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    public class CucuButton : MonoBehaviour
    {
        public UnityEvent OnClick => onClick;
        public UnityEvent<bool> OnFocusChanged => onFocusChanged;

        public bool Focus
        {
            get => focus;
            protected set
            {
                if (!Active) return;

                if (focus == value) return;

                focus = value;

                ExecuteOnFocusChange(value);

                OnFocusChanged.Invoke(value);
            }
        }

        public bool Active
        {
            get => active;
            set
            {
                if (value == false) Focus = false;
                active = value;
            }
        }

        [Space]
        [SerializeField] private bool active = true;

        [Header("Info")]
        [SerializeField] private bool focus;

        [Header("Events")]
        [SerializeField] private UnityEvent onClick = new UnityEvent();
        [SerializeField] private CucuBoolEvent onFocusChanged = new CucuBoolEvent();

        #region Public methods

        public void Click()
        {
            if (!Active) return;

            ExecuteOnClick();

            OnClick.Invoke();
        }

        #endregion

        #region Virtual

        protected virtual void ExecuteOnClick()
        {
        }

        protected virtual void ExecuteOnFocusChange(bool value)
        {
        }

        #endregion
    }
}