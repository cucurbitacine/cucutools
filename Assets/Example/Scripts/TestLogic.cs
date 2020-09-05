using CucuTools;
using UnityEngine;

namespace Example.Scripts
{
    public class TestLogic : MonoBehaviour
    {
        public Collider colliderCollision;
        public Collider colliderTrigger;

        public LayerMask layerMask;

        public CucuCollision collision;
        public CucuTrigger trigger;
    
        private void Start()
        {
            collision = new CucuCollision(colliderCollision) {LayerMask = layerMask};
            collision.OnEnter(coll => Debug.Log($"Collision enter {coll.gameObject.name}"));
        
            trigger = new CucuTrigger(colliderTrigger) {LayerMask = layerMask};
            trigger.OnEnter(coll => Debug.Log($"Trigger enter {coll.gameObject.name}"));
        }
    }
}