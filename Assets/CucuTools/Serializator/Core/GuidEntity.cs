using System;
using UnityEngine;

namespace CucuTools
{
    [Serializable]
    public class GuidEntity
    {
        public Guid Guid
        {
            get => UpdateGuid();
            set => guidString = value.ToString();
        }

        [SerializeField] private string guidString;

        public GuidEntity(Guid guid)
        {
            guidString = guid.ToString();
        }

        public GuidEntity()
        {
            guidString = Guid.NewGuid().ToString();
        }
        
        public static GuidEntity NewGuid()
        {
            return new GuidEntity();
        }

        public Guid UpdateGuid()
        {
            if (Guid.TryParse(guidString, out var guid) && guid != Guid.Empty) return guid;
            
            guid = Guid.NewGuid();
            guidString = guid.ToString();

            return guid;
        }

        public Guid UpdateGuid(string guid)
        {
            guidString = guid;
            
            return UpdateGuid();
        }
        
        public override string ToString()
        {
            return Guid.ToString();
        }
    }
}