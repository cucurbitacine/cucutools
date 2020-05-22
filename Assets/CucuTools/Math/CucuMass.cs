using System;
using UnityEngine;

namespace CucuTools
{
    [RequireComponent(typeof(Rigidbody))]
    public class CucuMass : MonoBehaviour, IMass
    {
        public new Rigidbody rigidbody
        {
            get => _rigidbody;
            private set => _rigidbody = value ?? throw new ArgumentNullException("rigidbody");
        }

        public float mass
        {
            get => rigidbody?.mass ?? throw new ArgumentNullException("rigidbody");
            set
            {
                if (rigidbody == null) throw new ArgumentNullException("rigidbody");
                rigidbody.mass = value;
            }
        }

        [SerializeField]
        private Rigidbody _rigidbody;

        private void Awake()
        {
            if (rigidbody == null)
                rigidbody = GetComponent<Rigidbody>();
        }
    }

    public interface IMass
    {
        float mass { get; set; }
    }
}