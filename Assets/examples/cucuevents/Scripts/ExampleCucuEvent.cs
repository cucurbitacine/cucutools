using cucu.tools;
using UnityEngine;

namespace cucu.example
{
    public class ExampleCucuEvent : MonoBehaviour
    {
        [Header("Void")]
        [SerializeField, Space]
        private KeyCode _keyCodeVoid       = KeyCode.Alpha1;

        [Header("Bool")]
        [SerializeField, Space]
        private KeyCode _keyCodeBool       = KeyCode.Alpha2;
        [SerializeField]
        private bool _valueBool = true;
        
        [Header("Int")]
        [SerializeField, Space]
        private KeyCode _keyCodeInt        = KeyCode.Alpha3;
        [SerializeField]
        private int _valueInt = 42;

        [Header("Float")]
        [SerializeField, Space]
        private KeyCode _keyCodeFloat      = KeyCode.Alpha4;
        [SerializeField]
        private float _valueFloat = 1.618f;

        [Header("String")]
        [SerializeField, Space]
        private KeyCode _keyCodeString     = KeyCode.Alpha5;
        [SerializeField]
        private string _valueString = "Hello world";

        [Header("Vector2")]
        [SerializeField, Space]
        private KeyCode _keyCodeVector2    = KeyCode.Alpha6;
        [SerializeField]
        private Vector2 _valueVector2 = Vector2.up;

        [Header("Vector3")]
        [SerializeField, Space]
        private KeyCode _keyCodeVector3    = KeyCode.Alpha7;
        [SerializeField]
        private Vector3 _valueVector3 = Vector3.up;

        [Header("Quaternion")]
        [SerializeField, Space]
        private KeyCode _keyCodeQuaternion = KeyCode.Alpha8;
        [SerializeField]
        private Quaternion _valueQuaternion = Quaternion.identity;

        [Header("Object")]
        [SerializeField, Space]
        private KeyCode _keyCodeObject     = KeyCode.Alpha9;
        private object _valueObject = new CucuEvent();

        private CucuEvent           cucuEvent           = new CucuEvent();
        private CucuBoolEvent       cucuBoolEvent       = new CucuBoolEvent();
        private CucuIntEvent        cucuIntEvent        = new CucuIntEvent();
        private CucuFloatEvent      cucuFloatEvent      = new CucuFloatEvent();
        private CucuStringEvent     cucuStringEvent     = new CucuStringEvent();
        private CucuVector2Event    cucuVector2Event    = new CucuVector2Event();
        private CucuVector3Event    cucuVector3Event    = new CucuVector3Event();
        private CucuQuaternionEvent cucuQuaternionEvent = new CucuQuaternionEvent();
        private CucuObjectEvent     cucuObjectEvent     = new CucuObjectEvent();

        private void Start()
        {
            cucuEvent          .AddListener(OnEvent);
            cucuBoolEvent      .AddListener(OnBoolEvent);
            cucuIntEvent       .AddListener(OnIntEvent);
            cucuFloatEvent     .AddListener(OnFloatEvent);
            cucuStringEvent    .AddListener(OnStringEvent);
            cucuVector2Event   .AddListener(OnVector2Event);
            cucuVector3Event   .AddListener(OnVector3Event);
            cucuQuaternionEvent.AddListener(OnQuaternionEvent);
            cucuObjectEvent    .AddListener(OnObjectEvent);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_keyCodeVoid))       cucuEvent          .Invoke();
            if (Input.GetKeyDown(_keyCodeBool))       cucuBoolEvent      .Invoke(_valueBool);
            if (Input.GetKeyDown(_keyCodeInt))        cucuIntEvent       .Invoke(_valueInt);
            if (Input.GetKeyDown(_keyCodeFloat))      cucuFloatEvent     .Invoke(_valueFloat);
            if (Input.GetKeyDown(_keyCodeString))     cucuStringEvent    .Invoke(_valueString);
            if (Input.GetKeyDown(_keyCodeVector2))    cucuVector2Event   .Invoke(_valueVector2);
            if (Input.GetKeyDown(_keyCodeVector3))    cucuVector3Event   .Invoke(_valueVector3);
            if (Input.GetKeyDown(_keyCodeQuaternion)) cucuQuaternionEvent.Invoke(_valueQuaternion);
            if (Input.GetKeyDown(_keyCodeObject))     cucuObjectEvent    .Invoke(_valueObject);
        }

        private void OnEvent()
        {
            Debug.Log($"OnEvent");
        }

        private void OnBoolEvent(bool value)
        {
            Debug.Log($"OnBoolEvent: {value}");
        }

        private void OnIntEvent(int value)
        {
            Debug.Log($"OnIntEvent: {value}");
        }

        private void OnFloatEvent(float value)
        {
            Debug.Log($"OnFloatEvent: {value}");
        }

        private void OnStringEvent(string value)
        {
            Debug.Log($"OnStringEvent: {value}");
        }

        private void OnVector2Event(Vector2 value)
        {
            Debug.Log($"OnVector2Event: {value}");
        }

        private void OnVector3Event(Vector3 value)
        {
            Debug.Log($"OnVector3Event: {value}");
        }

        private void OnQuaternionEvent(Quaternion value)
        {
            Debug.Log($"OnQuaternionEvent: {value}");
        }

        private void OnObjectEvent(object value)
        {
            Debug.Log($"OnObjectEvent: {value.GetType()}");
        }
    }
}