using UnityEngine;

namespace CucuTools
{
    [RequireComponent(typeof(Rigidbody))]
    public class CucuMass : MonoBehaviour, IMass
    {
        public Rigidbody rigidbody => _rigidbody;
        
        public float mass
        {
            get => rigidbody.mass;
            set => rigidbody.mass = value;
        }
        
        [SerializeField] private Rigidbody _rigidbody;

        private void Awake()
        {
            if (_rigidbody == null) 
                _rigidbody = GetComponent<Rigidbody>();
        }
    }

    public interface IMass
    {
        float mass { get; set; }
    }
}
