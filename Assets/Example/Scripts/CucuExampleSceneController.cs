using System;
using Cucu.Colors;
using Cucu.Log;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Examples.Scripts
{
    public class CucuExampleSceneController : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _value;

        [SerializeField] private Color _rainbowColor;
        [SerializeField] private Color _hotColor;
        [SerializeField] private Color _jetColor;
        [SerializeField] private Color _grayColor;
        [SerializeField] private Color _yargColor;

        [SerializeField] private GameObject _cube;
        [SerializeField] private GameObject _sphere;
        [SerializeField] private GameObject _capsule;

        private CucuLogger _cubeLogger;
        private CucuLogger _sphereLogger;
        private CucuLogger _capsuleLogger;
        
        private float _maxSpeed = float.Epsilon;
        
        private void Awake()
        {
            _cubeLogger = CucuLogger.Create()
                .SetTag(Color.blue, "cube")
                .SetType(LogType.Log)
                .SetArea(LogArea.Editor);

            _sphereLogger = CucuLogger.Create()
                .SetTag(Color.red, "sphere")
                .SetType(LogType.Log)
                .SetArea(LogArea.Editor);
            
            _capsuleLogger = CucuLogger.Create()
                .SetTag("capsule")
                .SetType(LogType.Warning)
                .SetArea(LogArea.Editor);
        }

        private void Update()
        {
            var speed = _capsule.GetComponent<Rigidbody>().velocity.magnitude;

            var color = CucuColor.Palettes.Hot.Get(speed / _maxSpeed);

            _capsule.GetComponent<Renderer>().material.color = color;
        }

        public void CubeAction()
        {
            var color = CucuColor.Palettes.Rainbow.Get(Random.value);
            _cube.GetComponent<Renderer>().material.color = color;
            _cubeLogger.Log("Action".ToColoredString(color));

            var speed = _capsule.GetComponent<Rigidbody>().velocity.magnitude;
            _maxSpeed = Mathf.Max(speed, _maxSpeed);
            _capsuleLogger.Log($"Speed : {speed}");
        }
        
        public void SphereAction()
        {
            var color = CucuColor.Palettes.Hot.Get(Random.value);
            _sphere.GetComponent<Renderer>().material.color = color;
            _sphereLogger.Log("Action".ToColoredString(color));
            
            var speed = _capsule.GetComponent<Rigidbody>().velocity.magnitude;
            _maxSpeed = Mathf.Max(speed, _maxSpeed);
            _capsuleLogger.Log($"Speed : {speed}");
        }
        
        private void OnValidate()
        {
            _rainbowColor = CucuColor.Palettes.Rainbow.Get(_value);
            _hotColor = CucuColor.Palettes.Hot.Get(_value);
            _jetColor = CucuColor.Palettes.Jet.Get(_value);
            _grayColor = CucuColor.Palettes.Gray.Get(_value);
            _yargColor = CucuColor.Palettes.Yarg.Get(_value);
        }
    }
}