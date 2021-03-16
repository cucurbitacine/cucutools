using UnityEngine;
using UnityEngine.Events;

namespace Example.Scripts
{
    public class InputHandler : MonoBehaviour
    {
        public KeyCode KeyCode = KeyCode.Space;

        public UnityEvent OnUp;
        public UnityEvent OnDown;
    
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode)) OnDown?.Invoke();
            if (Input.GetKeyUp(KeyCode)) OnUp?.Invoke();
        }
    }
}
