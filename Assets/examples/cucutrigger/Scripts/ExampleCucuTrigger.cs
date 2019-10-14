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

        public void TestSmaller(Transform transform)
        {
            Debug.Log($"[From editor smaller] : [{transform.gameObject.name}]");
            transform.localScale /= 2f;
        }

        public void TestBigger(Transform transform)
        {
            Debug.Log($"[From editor bigger] : [{transform.gameObject.name}]");
            transform.localScale *= 2f;
        }

        public void TestRotateX(Transform transform)
        {
            Debug.Log($"[From editor] : [{transform.gameObject.name}]");
            transform.Rotate(Vector3.right, 1.618f);
        }
    }
}