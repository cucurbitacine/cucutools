using UnityEngine;

namespace CucuTools.Statemachines.Core
{
    public abstract class ConditionEntity : MonoBehaviour
    {
        public abstract bool Done { get; set; }
    }
}