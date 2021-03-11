using UnityEngine;

namespace CucuTools.Statemachines.Core
{
    [DisallowMultipleComponent]
    public abstract class StateEntity : MonoBehaviour
    {
        public abstract bool IsPlaying { get; }

        public abstract bool IsLast { get; }

        public abstract bool TryGetNextState(out StateEntity nextState);

        public abstract void StartState();
        
        public abstract void StopState();
    }
}