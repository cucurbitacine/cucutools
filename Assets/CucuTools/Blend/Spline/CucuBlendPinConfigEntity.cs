using System.Collections.Generic;
using CucuTools;
using UnityEngine;

namespace CucuTools
{
    /// <inheritdoc />
    public abstract class CucuBlendPinConfigEntity<TBlendPin> : ScriptableObject, IBlendPinConfig<TBlendPin>
    {
        /// <inheritdoc />
        public string Key
        {
            get => _key;
            private set => _key = value;
        }

        /// <inheritdoc />
        public List<TBlendPin> Pins
        {
            get => _pins;
            private set => _pins = value;
        }

        [Header("Key of config")]
        [SerializeField] private string _key;

        [Header("Pins")]
        [SerializeField] private List<TBlendPin> _pins;
    }
}