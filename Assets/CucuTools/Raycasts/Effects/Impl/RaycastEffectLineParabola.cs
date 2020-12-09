using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    public class RaycastEffectLineParabola : RaycastEffectLine
    {
        private const int MIN_COUNT = 2;
        private const int MAX_COUNT = 64;

        private Vector3[] _cachedPoints;
        
        #region SerializeField

        [Header("Parabola settings")]
        [SerializeField] private AnimationCurve curve;
        [Range(MIN_COUNT, MAX_COUNT)]
        [SerializeField] private int resolution = 16;

        #endregion

        /// <inheritdoc />
        protected override void UpdateLine(bool hasHit, RaycastHit hit)
        {
            if (!hasHit)
            {
                line.enabled = false;
                return;
            }

            var pos = lineOrigin.position;
            var trg = hit.point;
            
            for (var i = 0; i < _cachedPoints.Length; i++)
            {
                var t = (float) i / (_cachedPoints.Length - 1);
                
                var point = Vector3.Lerp(pos, trg, t);
                
                var value = curve.Evaluate(t);
                point.y = Mathf.Lerp(trg.y, pos.y, value);
                
                _cachedPoints[i] = point;
            }
            
            line.SetPositions(_cachedPoints);

            line.enabled = true;
        }
        
        protected override void Validate()
        {
            base.Validate();

            if (curve == null) curve = GetDefaultCurve();

            resolution = Mathf.Clamp(resolution, MIN_COUNT, MAX_COUNT);
        }

        private AnimationCurve GetDefaultCurve()
        {
            return new AnimationCurve(new Keyframe(0f, 1f, 0f,0f), new Keyframe(1f, 0f, -2f,0f));
        }
        
        #region MonoBehaviour

        private void Awake()
        {
            _cachedPoints = new Vector3[resolution];
            line.positionCount = resolution;
            
            Validate();
        }

        protected override void Reset()
        {
            curve = GetDefaultCurve();

            base.Reset();
        }

        #endregion
    }
}