using CucuTools.Statemachines.Core;
using UnityEngine;

namespace CucuTools.Statemachines
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