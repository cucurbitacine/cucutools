using System;
using UnityEngine;
using UnityEngine.Events;

namespace Example.Scripts
{
    public class InputBehaviour : MonoBehaviour
    {
        public InputUnit InputUnit => inputUnit ?? (inputUnit = new InputUnit());
        
        [SerializeField] private InputUnit inputUnit;

        private void Update()
        {
            if (InputUnit.GetKey()) InputUnit.Invoke();
        }
    }

    [Serializable]
    public class InputUnit
    {
        public UnityEvent Event => @event ?? (@event = new UnityEvent());
        
        public bool IsEnabled = true;
        public InputMode InputMode = InputMode.Down;
        public KeyCode KeyCode = KeyCode.Space;
        
        [SerializeField] private UnityEvent @event;
        
        public static bool GetKey(InputMode inputMode, KeyCode keyCode)
        {
            switch (inputMode)
            {
                case InputMode.Down: return Input.GetKeyDown(keyCode);
                case InputMode.Stay: return Input.GetKey(keyCode);
                case InputMode.Up: return Input.GetKeyUp(keyCode);
                default: return false;
            }
        }

        public void Invoke()
        {
            Event.Invoke();
        }
        
        public bool GetKey(InputMode inputMode)
        {
            return IsEnabled && GetKey(inputMode, KeyCode);
        }
        
        public bool GetKey(KeyCode keyCode)
        {
            return IsEnabled && GetKey(InputMode, keyCode);
        }
        
        public bool GetKey()
        {
            return IsEnabled && GetKey(InputMode, KeyCode);
        }
    }
    
    public enum InputMode
    {
        Down,
        Stay,
        Up
    }
}
