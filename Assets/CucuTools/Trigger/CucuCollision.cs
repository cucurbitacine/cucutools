using System;
using UnityEngine;

namespace CucuTools
{
    [Serializable]
    public class CucuCollision : ICucuCollision<CucuCollision>
    {
        [SerializeField] private CucuCollisionBehaviour _collisionBehaviour;

        public CucuCollision(CucuCollisionBehaviour collisionBehaviour)
        {
            _collisionBehaviour = collisionBehaviour;
        }
        
        public CucuCollision(Collider collider) : this(collider.gameObject.AddComponent<CucuCollisionBehaviour>())
        {
            _collisionBehaviour.SetCollider(collider);
        }

        public bool Active => _collisionBehaviour.Active;
        public LayerMask LayerMask => _collisionBehaviour.LayerMask;

        public CucuCollision SetActive(bool value = true)
        {
            _collisionBehaviour.SetActive(value);
            return this;
        }

        public CucuCollision SetLayerMask(LayerMask layerMask)
        {
            _collisionBehaviour.SetLayerMask(layerMask);
            return this;
        }

        public CucuCollision OnEnter(params Action<Collision>[] actions)
        {
            _collisionBehaviour.OnEnter(actions);
            return this;
        }

        public CucuCollision OnStay(params Action<Collision>[] actions)
        {
            _collisionBehaviour.OnStay(actions);
            return this;
        }

        public CucuCollision OnExit(params Action<Collision>[] actions)
        {
            _collisionBehaviour.OnExit(actions);
            return this;
        }
    }
}