using UnityEngine;

namespace CucuTools
{
    public class GuidBehavour : MonoBehaviour
    {
        public GuidEntity GuidEntity => guidEntity ?? (guidEntity = new GuidEntity());
        
        [SerializeField] private GuidEntity guidEntity;

        [ContextMenu("Log")]
        public void Log()
        {
            Debug.Log(GuidEntity.ToString());
        }
        
        protected virtual void OnValidate()
        {
            GuidEntity.UpdateGuid();
        }
    }
}