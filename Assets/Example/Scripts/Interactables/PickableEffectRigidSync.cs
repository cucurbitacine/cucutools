using CucuTools.Interactables.Pickables;
using CucuTools.Math;
using UnityEngine;

namespace Example.Scripts.Interactables
{
    [DisallowMultipleComponent]
    public class PickableEffectRigidSync : PickableEffect
    {
        public CucuRigidSync RigidSync
        {
            get
            {
                if (rigidSync != null) return rigidSync;
                rigidSync = GetComponent<CucuRigidSync>();
                if (rigidSync == null)
                {
                    rigidSync = gameObject.AddComponent<CucuRigidSync>();
                    rigidSync.IsEnabled = false;
                }
                return rigidSync;
            }
        }
        
        [Header("RigidSync")]
        [SerializeField] private CucuRigidSync rigidSync;

        protected override void PickInternal()
        {
            RigidSync.TargetSync = Pickable.Ownership.Owner.transform;
            RigidSync.TargetSync.position = transform.position;
            RigidSync.TargetSync.rotation = transform.rotation;
            RigidSync.IsEnabled = true;
        }

        protected override void ThrowInternal()
        {
            RigidSync.TargetSync = null;
            RigidSync.IsEnabled = false;
        }
    }
}