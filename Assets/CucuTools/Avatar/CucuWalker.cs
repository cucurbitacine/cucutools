using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    [RequireComponent(typeof(CharacterController))]
    public class CucuWalker : MonoBehaviour
    {
        #region Public

        public bool Active
        {
            get => active;
            set => active = value;
        }

        public bool CanWalk
        {
            get => canWalk;
            set => canWalk = value;
        }

        public bool CanJump
        {
            get => canJump;
            set => canJump = value;
        }
        
        public bool UseGravity
        {
            get => useGravity;
            set => useGravity = value;
        }
        
        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        public UnityEvent<float> OnJumpEvent { get; } = new CucuFloatEvent();

        #endregion

        #region Private

        private float Gravity => Physics.gravity.y;
        [Space]
        [SerializeField] private bool active = true;
        
        [Header("Settings")]
        [SerializeField] private bool canWalk = true;
        [SerializeField] private bool canJump = true;
        [SerializeField] private bool useGravity = true;
        [SerializeField] [Range(0f, 20f)] private float speed = 3f;
        [SerializeField] [Range(0f, 03f)] private float jumpHeight = 0.5f;

        [Header("References")]
        [SerializeField] private CharacterController character;

        [Header("Info")]
        [SerializeField] private Vector2 inputInfo;
        [SerializeField] private float velocityYInfo;
        [SerializeField] private bool isGroundedInfo;

        private float horizontalInput;
        private float verticalInput;
        private float velocityY;
        private bool jumpInput;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            character = GetComponent<CharacterController>();
        }

        private void Update()
        {
            UpdateInput();

            UpdateMovements(Time.deltaTime);

#if UNITY_EDITOR
            UpdateInfo();
#endif
        }

        #endregion

        #region Update methods

        private void UpdateMovements(float deltaTime)
        {
            if (!Active) return;

            var move = Vector3.zero;
            move += GetXZMovement(deltaTime);
            move += GetYMovement(deltaTime);

            character.Move(move * deltaTime);
        }

        private void UpdateInput()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            jumpInput = Input.GetButtonDown("Jump");
        }

        private void UpdateInfo()
        {
            inputInfo = new Vector2(horizontalInput, verticalInput);
            velocityYInfo = velocityY;
            isGroundedInfo = character.isGrounded;
        }

        #endregion

        #region Caclulate movements

        private Vector3 GetXZMovement(float deltaTime)
        {
            return CanWalk
                ? speed * (horizontalInput * character.transform.right + verticalInput * character.transform.forward)
                : Vector3.zero;
        }

        private Vector3 GetYMovement(float deltaTime)
        {
            if (character.isGrounded)
            {
                if (UseGravity && CanJump && jumpInput)
                {
                    velocityY = Mathf.Sqrt(jumpHeight * 2f * -Gravity);
                    OnJumpEvent.Invoke(jumpHeight);
                }
                //else if(velocityY < 0f) velocityY = 0.0f; TODO doesn't work...
            }
            else
            {
                if (UseGravity)
                    velocityY += Gravity * deltaTime;
                else
                    velocityY = 0.0f;
            }

            return Vector3.up * velocityY;
        }

        #endregion
    }
}