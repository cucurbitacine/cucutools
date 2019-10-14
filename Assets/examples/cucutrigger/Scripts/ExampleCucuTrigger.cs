using UnityEngine;
using cucu.tools;

namespace cucu.example
{
    public class ExampleCucuTrigger : MonoBehaviour
    {
        [SerializeField] private CucuTrigger _trigger;

        private void Start()
        {
            _trigger.RegisterComponent<Transform>().AddListener(obj =>
            {
                var transform = obj as Transform;
                transform.localScale /= 0.5f;

                Debug.Log($"[From code enter] : [{transform.gameObject.name}] : [{obj.GetType()}]");
            });

            _trigger.RegisterComponent<Transform>(CucuTrigger.TriggerState.Exit).AddListener(obj =>
            {
                var transform = obj as Transform;
                transform.localScale *= 0.5f;

                Debug.Log($"[From code exit] : [{transform.gameObject.name}] : [{obj.GetType()}]");
            });
        }

        public void TestLeft(Transform transform)
        {
            Debug.Log($"[From editor] : [{transform.gameObject.name}]");
            transform.position += Vector3.left;
        }

        public void TestRight(Transform transform)
        {
            Debug.Log($"[From editor] : [{transform.gameObject.name}]");
            transform.position += Vector3.right;
        }

        public void TestRotateX(Transform transform)
        {
            Debug.Log($"[From editor] : [{transform.gameObject.name}]");
            transform.Rotate(Vector3.right, 1.618f);
        }
    }
}