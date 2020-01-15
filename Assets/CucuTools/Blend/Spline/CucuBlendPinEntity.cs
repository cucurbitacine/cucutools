using System;
using CucuTools.Blend.Interfaces;
using UnityEngine;

namespace CucuTools.Blend.Spline
{
    /// <inheritdoc />
    [Serializable]
    public abstract class CucuBlendPinEntity<TObject> : IBlendPin<TObject>
    {
        /// <inheritdoc />
        public string Key
        {
            get => _key;
            set => _key = value;
        }

        /// <inheritdoc />
        public float Value       
        {
            get => _value;
            set => _value = value;
        }

        /// <inheritdoc />
        public TObject Pin
        {
            get => _pin;
            set => _pin = value;
        }

        [SerializeField] private string _key;
        [Range(0f, 1f)] [SerializeField] private float _value;
        [SerializeField] private TObject _pin;

        public CucuBlendPinEntity() : this(0.0f, default(TObject))
        {
        }

        public CucuBlendPinEntity(float value, TObject pin)
        {
            _value = value;
            _pin = pin;
        }
    }
}