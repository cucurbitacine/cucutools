using UnityEngine;

namespace CucuTools.Statemachines.Core
{
    public abstract class ConditionEntity : MonoBehaviour
    {
        public abstract bool Done { get; set; }

        public void Set(bool value)
        {
            Done = value;
        }
        
        public void True()
        {
            Set(true);
        }
        
        public void False()
        {
            Set(false);
        }
        
        public void Toggle()
        {
            Set(!Done);
        }
    }
}