using UnityEngine;
using UnityEngine.Events;

namespace Example.Scripts
{
    [DisallowMultipleComponent]
    public class InteracableBehavior : MonoBehaviour
    {
        public UnityEvent OnNormal;
        public UnityEvent OnHover;
        public UnityEvent OnClick;
    
        public virtual void Normal()
        {
            OnNormal.Invoke();
        }
        
        public virtual void Hover()
        {
            OnHover.Invoke();
        }

        public virtual void Click()
        {
            OnClick.Invoke();
        }
    }
}