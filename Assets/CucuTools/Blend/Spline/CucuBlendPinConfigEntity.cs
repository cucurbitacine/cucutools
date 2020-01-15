using System.Collections.Generic;
using CucuTools.Blend.Interfaces;
using UnityEngine;

namespace CucuTools.Blend.Spline
{
    /// <inheritdoc />
    public abstract class CucuBlendPinConfigEntity<TBlendPin> : ScriptableObject, IBlendPinConfig<TBlendPin>
    {
        /// <inheritdoc />
        public string Key => _key;

        /// <inheritdoc />
        public List<TBlendPin> Pins => _pins;

        [Header("Key of config")]
        [SerializeField] private string _key;

        [Header("Pins")]
        [SerializeField] private List<TBlendPin> _pins;
    }
}