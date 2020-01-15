using UnityEngine;

namespace CucuTools.Blend.Impl
{
    /// <inheritdoc />
    public class CucuBlendValueCurve : CucuBlendEntity
    {
        public override string Key => "Value curve blend";
        
        public float Value => _value;
        
        [Header("Root curve")]
        [SerializeField] private AnimationCurve _curve;
        
        [Header("Default value")]
        [SerializeField] private float _valueDefault = 0f;
        
        [Header("Multiplier value")]
        [SerializeField] private float _modifier = 1f;
        
        [Header("Result")]
        [SerializeField] private float _value;
        
        protected override void UpdateEntity()
        {
            _value = _valueDefault + _curve.Evaluate(Blend) * _modifier;
        }
    }
}