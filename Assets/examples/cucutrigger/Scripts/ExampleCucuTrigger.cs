using UnityEngine;
using cucu.tools;

namespace cucu.example
{
    public class ExampleCucuTrigger : MonoBehaviour
    {
        [SerializeField] private CucuTrigger _trigger;
        private void Start()
        {
            var types = _trigger.RegisteredTypesList;
            foreach(var type in types)
            {
                _trigger.RegisterComponent(type).AddListener(obj=>
                {
                    var gameObject = (obj as Component).gameObject;

                    Debug.Log($"[From editor] : [{gameObject.name}] : [{obj.GetType()}]");
                });
            }

            _trigger.RegisterComponent<Transform>().AddListener(obj=>
            {
                var transform = obj as Transform;
                transform.rotation = Random.rotation;

                Debug.Log($"[From code] : [{transform.gameObject.name}] : [{obj.GetType()}]");
            });
        }
    }
}


