using UnityEngine;

namespace CucuTools
{
    public class CucuViewer : MonoBehaviour
    {
        private const float SENSITIVITY_MIN = 0.001f;
        private const float SENSITIVITY_MAX = 10f;

        public float SensitivityHorizontal => sensitivityHorizontal;
        public float SensitivityVertical => sensitivityVertical;

        #region Serialized fields
        
        [Space]
        [SerializeField] private bool active = true;
        [SerializeField] private bool cursorVisible;
        
        [Header("Settings")]
        [Range(SENSITIVITY_MIN, SENSITIVITY_MAX)]
        [SerializeField] private float sensitivityHorizontal = 2;
        [Range(SENSITIVITY_MIN, SENSITIVITY_MAX)]
        [SerializeField] private float sensitivityVertical = 2;
        [Range(0f, 90f)]
        [SerializeField] private float yFieldViewMax = 90f;
        [Range(-90f, 0f)]
        [SerializeField] private float yFieldViewMin = -90f;

        [Header("References")]
        [Tooltip("If is null, get self transform")]
        [SerializeField] private Transform horizontalTransform;
        [Tooltip("If is null, try get child camera or self transform")]
        [SerializeField] private Transform verticalTransform;

        #endregion
        
        [Header("Info")]
        [SerializeField] private Vector2 view;
        [SerializeField] private float mouseXInput;
        [SerializeField] private float mouseYInput;
        
        private void Awake()
        {
            if (horizontalTransform == null)
                SetHorizontalTransform(transform);
            if (verticalTransform == null)
                SetVerticalTransform(GetComponentInChildren<Camera>()?.transform ?? transform);

            Cursor.lockState = CursorLockMode.Confined;

            SetupStartRotation();
        }

        private void Update()
        {
            UpdateInput();
            
            UpdateViewer(Time.deltaTime);
        }

        public void SetSensitivity(float axisX, float axisY)
        {
            SetSensitivityAxisX(axisX);
            SetSensitivityAxisY(axisY);
        }

        public void SetSensitivityAxisX(float axisX)
        {
            sensitivityHorizontal = Mathf.Clamp(axisX, SENSITIVITY_MIN, SENSITIVITY_MAX);
        }

        public void SetSensitivityAxisY(float axisY)
        {
            sensitivityVertical = Mathf.Clamp(axisY, SENSITIVITY_MIN, SENSITIVITY_MAX);
        }

        public void SetActive(bool value)
        {
            active = value;
        }

        private void SetupStartRotation()
        {
            view = new Vector2(horizontalTransform.localRotation.eulerAngles.y, verticalTransform.localRotation.eulerAngles.x);
        }
        
        public void Look(Vector2 look)
        {
            if (!Active) return;
            
            view += new Vector2(look.x * SensitivityHorizontal, look.y * SensitivityVertical);
            
            ValidationView(ref view);
            
            Look(view.x, view.y);
        }

        private void Look(float angleHorizontal, float angleVertical)
        {
            ValidationAngles(ref angleHorizontal, ref angleVertical);

            var eulerLeftRight = horizontalTransform.localRotation.eulerAngles;
            var eulerDownUp = verticalTransform.localRotation.eulerAngles;

            horizontalTransform.localRotation = Quaternion.Euler(eulerLeftRight.x, angleHorizontal, eulerLeftRight.z);
            verticalTransform.localRotation = Quaternion.Euler(-angleVertical, eulerDownUp.y, eulerDownUp.z);
        }

        private void ValidationView(ref Vector2 view)
        {
            var x = view.x;
            var y = view.y;

            ValidationAngles(ref x, ref y);

            view.x = x;
            view.y = y;
        }

        private void ValidationAngles(ref float horizontal, ref float vertical)
        {
            if (horizontal > 360) horizontal -= 360;
            if (horizontal < 0) horizontal += 360;

            if (vertical > yFieldViewMax) vertical = yFieldViewMax;
            if (vertical < yFieldViewMin) vertical = yFieldViewMin;
        }

        public bool Active
        {
            get => active;
            set => SetActive(value);
        }

        public Vector2 SensitivityView
        {
            get => new Vector2(sensitivityHorizontal, sensitivityVertical);
            set
            {
                sensitivityHorizontal = value.x;
                sensitivityVertical = value.y;
            }
        }

        public void SetHorizontalTransform(Transform transform)
        {
            horizontalTransform = transform;
        }
        
        public void SetVerticalTransform(Transform transform)
        {
            verticalTransform = transform;
        }

        private void UpdateViewer(float deltaTime)
        {
            if (!Active) return;
            
            Cursor.visible = cursorVisible;
            
            UpdateRotation(deltaTime);
        }

        private void UpdateInput()
        {
            mouseXInput = Input.GetAxis("Mouse X");
            mouseYInput = Input.GetAxis("Mouse Y");
        }

        private void UpdateRotation(float deltaTime)
        {
            var look = new Vector2(mouseXInput, mouseYInput) * deltaTime * 100f;

            Look(look);
        }
    }
}