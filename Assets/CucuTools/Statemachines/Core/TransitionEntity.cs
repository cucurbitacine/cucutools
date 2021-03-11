using UnityEngine;

namespace CucuTools.Statemachines.Core
{
    public abstract class TransitionEntity : MonoBehaviour
    {
        public abstract bool IsReady { get; }

        public abstract StateEntity Target { get; set; }
        
        public abstract StateEntity Owner { get; }
    }
}