using UnityEngine;

namespace CucuTools
{
    [RequireComponent(typeof(Rigidbody))]
    public class CucuRigidSync : MonoBehaviour
    {
        private static PhysicMaterial _rigidSyncPhysicMaterial;
    
        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }
    
        public Transform TargetSync
        {
            get => targetSync;
            set => targetSync = value;
        }
    
        public Rigidbody Rigidbody
        {
            get => rigid;
            protected set => rigid = value;
        }

        [Header("Main Settings")]
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private Transform targetSync;

        [Header("Additional Settings")]
        [SerializeField] private bool syncPosition = true;
        [SerializeField] private bool syncRotation = true;
        [Range(0f, 1000f)]
        [SerializeField] private float maxVelocity = 500f;
        [Range(0f, 1f)]
        [SerializeField] private float syncWeight = 1f;
    
        [Header("References")]
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private Collider[] colliders;
    
        public bool IsValid()
        {
            return TargetSync != null && Rigidbody != null;
        }

        private void ValidateRigidbody()
        {
            if (Rigidbody == null) SetupRigidbody();
        
            Rigidbody.useGravity = false;
            Rigidbody.isKinematic = false;

            Rigidbody.drag = 0f;
            Rigidbody.angularDrag = 0f;

            Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            Rigidbody.maxDepenetrationVelocity = 10f;
        }
    
        private void ValidateColliders()
        {
            if (colliders == null) SetupColliders();

            foreach (var collider in colliders)
                ValidateCollider(collider);
        }

        private void ValidateCollider(Collider collider)
        {
            if (_rigidSyncPhysicMaterial == null)
            {
                _rigidSyncPhysicMaterial = new PhysicMaterial("rigidsync");
                _rigidSyncPhysicMaterial.bounciness = 0f;
                _rigidSyncPhysicMaterial.dynamicFriction = 0f;
                _rigidSyncPhysicMaterial.staticFriction = 0f;
                _rigidSyncPhysicMaterial.frictionCombine = PhysicMaterialCombine.Average;
            }
        
            if (collider == null) return;
            
            collider.sharedMaterial = _rigidSyncPhysicMaterial;
            collider.isTrigger = false;
        }
    
        [CucuButton("Setup Rigidbody", @group: "Setup")]
        private void SetupRigidbody()
        {
            Rigidbody = GetComponent<Rigidbody>();
            if (Rigidbody == null) Rigidbody = gameObject.AddComponent<Rigidbody>();
        }
    
        [CucuButton("Setup Colliders", @group: "Setup")]
        private void SetupColliders()
        {
            colliders = GetComponentsInChildren<Collider>();
        }
    
        private void Validate()
        {
            ValidateRigidbody();
            ValidateColliders();
        }

        private void Sync(float deltaTime)
        {
            if (syncPosition)
            {
                var dPos = TargetSync.position - transform.position;
                Rigidbody.velocity = Vector3.Lerp(Rigidbody.velocity, Vector3.ClampMagnitude(dPos / deltaTime, maxVelocity),
                    syncWeight);
            }

            if (syncRotation)
            {
                var from = transform.rotation;
                var to = TargetSync.rotation;
                var conj = new Quaternion(-from.x, -from.y, -from.z, from.w);
                var dq = new Quaternion((to.x - from.x) * 2.0f, 2.0f * (to.y - from.y), 2.0f * (to.z - from.z),
                    2.0f * (to.w - from.w));
                var c = dq * conj;
                var dRot = new Vector3(c.x, c.y, c.z);

                Rigidbody.angularVelocity = Vector3.Lerp(Rigidbody.angularVelocity, dRot / deltaTime, syncWeight);
            }
        }

        protected virtual void Awake()
        {
            Validate();
        }

        protected void FixedUpdate()
        {
            if (IsEnabled) Sync(Time.fixedDeltaTime);
        }

        protected virtual void OnValidate()
        {
            if (!Application.isPlaying) Validate();
        }
    }
}