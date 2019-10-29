using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace cucu.tools
{
    public class CucuTag : MonoBehaviour
    {
        public readonly static IList<CucuTag> Tags = new List<CucuTag>();

        public GUID Guid => (GUID) (_guid ?? (_guid = GUID.Generate()));
        private GUID? _guid;

        public string Key
        {
            get => _key;
            set => _key = value;
        }

        [SerializeField] private string _key;

        private void OnEnable()
        {
            Tags.Add(this);
        }

        private void OnDisable()
        {
            Tags.Remove(this);
        }

        private void OnDestroy()
        {
            Tags.Remove(this);
        }
    }
}