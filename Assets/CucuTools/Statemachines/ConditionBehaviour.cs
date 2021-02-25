using UnityEngine;

namespace CucuTools
{
    public class ConditionBehaviour : ConditionEntity
    {
        public override bool Done
        {
            get => done;
            set => done = value;
        }
        
        [SerializeField] private bool done;
    }
}