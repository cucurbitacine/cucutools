using UnityEngine;

namespace CucuTools
{
    public abstract class TransitionEntity : MonoBehaviour
    {
        public abstract bool IsReady { get; }

        public abstract StateEntity Target { get; set; }
        
        public abstract StateEntity Owner { get; }
    }
}