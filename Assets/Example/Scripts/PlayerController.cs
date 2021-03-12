using System;
using System.Linq;
using UnityEngine;

namespace Example.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        public bool isEnabled = true;

        [SerializeField] private float speed = 250f;
        [SerializeField] private float speedView = 1f;
        
        [SerializeField] private Vector3 move;
        [SerializeField] private Vector2 view;
        
        [SerializeField] private Transform head;
        
        private Rigidbody rigid;
        

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
            x = Mathf.Clamp(x, -60f, 60f);
            head.localRotation = Quaternion.Euler(Vector3.right * x);
        }

        private void LimitSpeed()
        {
            rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, speed);
        }
        
        private void Validate()
        {
            rigid = GetComponent<Rigidbody>();

            rigid.constraints = RigidbodyConstraints.FreezeRotation;

            head = GetComponentsInChildren<Transform>().FirstOrDefault(t => t.name.ToLower() == "head");
            if (head == null) head = transform;
        }
    
        private void Awake()
        {
            Validate();

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            UpdateInput();
        }

        private void LateUpdate()
        {
            if (isEnabled) UpdateView(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (isEnabled) UpdateMove(Time.deltaTime);
        }

        private void OnValidate()
        {
            Validate();
        }
    }
}
