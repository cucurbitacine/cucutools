using UnityEngine;

namespace CucuTools.Common
{
    public class GuidBehavour : MonoBehaviour
    {
        public GuidEntity GuidEntity => guidEntity ?? (guidEntity = new GuidEntity());
        
        [SerializeField] private GuidEntity guidEntity;

        protected virtual void OnValidate()
        {
            GuidEntity.UpdateGuid();
        }
    }
}