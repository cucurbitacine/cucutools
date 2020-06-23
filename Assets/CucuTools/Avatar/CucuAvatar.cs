using UnityEngine;

namespace CucuTools
{
    public class CucuAvatar : MonoBehaviour
    {
        public float speed = 1;
        public float speedX = 1;
        public float speedY = 1;

        public float moveX;
        public float moveY;
        
        public Transform rootX;
        public Transform rootY;

        private float mouseX;
        private float mouseY;
        
        private void Awake()
        {
            if (rootX == null) rootX = transform;
            if (rootY == null) rootY = GetComponentInChildren<Camera>().transform;
            if (rootY == null) rootY = transform;
        }

        private void Update()
        {
            UpdateInput();

            if (Input.GetKey(KeyCode.Mouse1))
            {
                Cursor.visible = false;
                UpdateRotation();
            }
            else
            {
                Cursor.visible = true;
            }

            UpdatePosition();
        }

        private void UpdateInput()
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
            
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        private void UpdatePosition()
        {
            var shift = new Vector3(moveX, 0, moveY);
            if (shift.sqrMagnitude > 1f) shift.Normalize();
            rootX.Translate(shift * speed * Time.deltaTime);
        }

        private void UpdateRotation()
        {
            rootX.RotateAround(Vector3.up, speedX * mouseX * Time.deltaTime);
            rootY.rotation = rootY.rotation;
            rootY.RotateAround(rootY.right, -speedY * mouseY * Time.deltaTime);
        }
    }
}
