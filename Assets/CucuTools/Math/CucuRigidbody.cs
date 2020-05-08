using UnityEngine;

namespace CucuTools
{
    [RequireComponent(typeof(CucuTracker))]
    [RequireComponent(typeof(CucuMass))]
    public class CucuRigidbody : MonoBehaviour, IRigid
    {
        public CucuTracker tracking => _tracker;
        public CucuMass body => _mass;
        
        private CucuTracker _tracker;
        private CucuMass _mass;

        public float mass
        {
            get => body.mass;
            set => body.mass = value;
        }

        public Vector3 position => tracking.position;
        public Vector3 velocity => tracking.velocity;
        public Vector3 acceleration => tracking.acceleration;

        private void Awake()
        {
            _tracker = GetComponent<CucuTracker>();
            _mass = GetComponent<CucuMass>();
        }
    }

    public interface IRigid : IMass, ITracker
    {
    
    }
}