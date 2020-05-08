using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public class CucuTracker : MonoBehaviour, ITracker
    {
        public Vector3 position =>
            smoothAll && smoothPos && prevPosSmooth.Count > 0
                ? (prevPosSmooth.Aggregate((res, curr) => res + curr) /
                   prevPosSmooth.Count)
                : currPos;

        public Vector3 velocity =>
            smoothAll && smoothVel && prevVelSmooth.Count > 0
                ? (prevVelSmooth.Aggregate((res, curr) => res + curr) /
                   prevVelSmooth.Count)
                : currVel;

        public Vector3 acceleration =>
            smoothAll && smoothAcc && prevAccSmooth.Count > 0
                ? (prevAccSmooth.Aggregate((res, curr) => res + curr) /
                   prevAccSmooth.Count)
                : currAcc;

        private int countElementSmoothing => _countElementSmoothing;
        private float durationSmoothing => _durationSmoothing;

        [Header("Params")]
        [SerializeField] private bool fixedUpdate;
        
        [Header("Smooth")]
        [SerializeField] private bool smoothAll;
        [SerializeField] private bool smoothPos;
        [SerializeField] private bool smoothVel;
        [SerializeField] private bool smoothAcc;
        [SerializeField] private float _durationSmoothing;
        [SerializeField, Range(1, 256)] private int _countElementSmoothing;
        
        
        [Header("Current values")]
        [SerializeField] private Vector3 currPos;
        [SerializeField] private Vector3 currVel;
        [SerializeField] private Vector3 currAcc;
        
        private Vector3 prevPos;
        private Vector3 prevVel;

        private List<Vector3> prevPosSmooth = new List<Vector3>();
        private List<Vector3> prevVelSmooth = new List<Vector3>();
        private List<Vector3> prevAccSmooth = new List<Vector3>();

        [SerializeField, Range(0f, 1f)] private float posTol;
        [SerializeField, Range(0f, 1f)] private float velTol;
        
        [SerializeField] private int index;
        
        private Vector3 GetDelta(Vector3 delta, float tolerance)
        {
            var x = Mathf.Abs(delta.x) >= tolerance ? delta.x : 0f;//delta.x * Mathf.Exp(delta.x - tolerance);
            var y = Mathf.Abs(delta.y) >= tolerance ? delta.y : 0f;//delta.y * Mathf.Exp(delta.y - tolerance);
            var z = Mathf.Abs(delta.z) >= tolerance ? delta.z : 0f;//delta.z * Mathf.Exp(delta.z - tolerance);

            return new Vector3(x, y, z);
        }
        
        private void Awake()
        {
            prevPos = transform.position;
            prevVel = Vector3.zero;
        }
        
        private void Update()
        {
            if (fixedUpdate) return;
            
            UpdateInternal(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (!fixedUpdate) return;

            UpdateInternal(Time.fixedDeltaTime);
        }

        public CucuTracker SetCountSmooth(int count)
        {
            _countElementSmoothing = count < 1 ? 1 : count;
            return this;
        }

        private void UpdateInternal(float dt)
        {
            _durationSmoothing = countElementSmoothing * dt;

            UpdateValues(dt);

            if (smoothAll) UpdateSmoothing();
        }

        private void UpdateValues(float dt)
        {
            currPos = transform.position;
            currVel = GetDelta(currPos - prevPos, posTol) / dt;
            currAcc = GetDelta(currVel - prevVel, velTol) / dt;

            prevPos = currPos;
            prevVel = currVel;
        }

        private void UpdateSmoothing()
        {
            index %= countElementSmoothing;

            if (prevPosSmooth.Count <= index)
                prevPosSmooth.Add(currPos);
            else
                prevPosSmooth[index] = currPos;

            while (prevPosSmooth.Count > countElementSmoothing)
                prevPosSmooth.RemoveAt(prevPosSmooth.Count - 1);

            //
            
            if (prevVelSmooth.Count <= index)
                prevVelSmooth.Add(currVel);
            else
                prevVelSmooth[index] = currVel;

            while (prevVelSmooth.Count > countElementSmoothing)
                prevVelSmooth.RemoveAt(prevVelSmooth.Count - 1);
            
            //
            
            if (prevAccSmooth.Count <= index)
                prevAccSmooth.Add(currAcc);
            else
                prevAccSmooth[index] = currAcc;

            while (prevAccSmooth.Count > countElementSmoothing)
                prevAccSmooth.RemoveAt(prevAccSmooth.Count - 1);
            
            index++;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, position);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(position, position + velocity);
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, position + acceleration);
        }
    }

    public interface ITracker
    {
        Vector3 position { get; }
        Vector3 velocity { get; }
        Vector3 acceleration { get; }
    }
}