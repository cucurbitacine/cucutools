using System.Linq;
using UnityEngine;

namespace Example.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;

                foreach (var inputHandler in GetComponentsInChildren<InputBehaviour>())
                {
                    inputHandler.enabled = isEnabled;
                }
                
                cam.enabled = isEnabled;
                if (bodies != null)
                    foreach (var body in bodies)
                        body?.SetActive(!isEnabled);
            }
        }
        [SerializeField] private bool isEnabled = true;

        [SerializeField] private float speed = 250f;
        [SerializeField] private float speedView = 1f;
        
        [SerializeField] private Vector3 move;
        [SerializeField] private Vector2 view;
        
        [SerializeField] private Transform head;
        [SerializeField] private GameObject[] bodies;
        
        private Rigidbody rigid;
        private Camera cam;
        
        private void UpdateInput()
        {
            move.x = Input.GetAxis("Horizontal");
            move.y = 0f;
            move.z = Input.GetAxis("Vertical");

            view.x = Input.GetAxis("Mouse X");
            view.y = Input.GetAxis("Mouse Y");
        }

        private void UpdateMove(float dt)
        {
            rigid.velocity = speed * rigid.transform.TransformDirection(Vector3.ClampMagnitude(move, 1));

            LimitSpeed();
        }
        
        private void UpdateView(float dt)
        {
            rigid.transform.Rotate(rigid.transform.up, view.x * speedView * dt);

            var x = head.localRotation.eulerAngles.x - view.y * speedView * dt;
            if (x > 180f) x -= 360f;
            x = Mathf.Clamp(x, -90f, 90f);
            head.localRotation = Quaternion.Euler(Vector3.right * x);
        }

        private void LimitSpeed()
        {
            rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, speed);
        }
        
        private void Validate()
        {
            cam = GetComponentInChildren<Camera>();
            
            rigid = GetComponent<Rigidbody>();
            
            rigid.constraints = RigidbodyConstraints.FreezeRotation;

            head = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name.ToLower() == "head");
            if (head == null) head = transform;
        }
    
        private void Awake()
        {
            Validate();

            IsEnabled = isEnabled;

            if (IsEnabled) Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            UpdateInput();
            
            if (IsEnabled) UpdateView(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (IsEnabled) UpdateMove(Time.deltaTime);
        }

        private void OnValidate()
        {
            Validate();
        }
    }
}
