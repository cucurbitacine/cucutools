using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools
{
    public abstract class WaiterEntity : CustomYieldInstruction, IWaiterEntity
    {
        public override bool keepWaiting => !IsDone;

        public abstract bool IsDone { get; }
        public abstract bool Success { get; }

        public UnityEvent OnCompleted => _onCompleted ?? (_onCompleted = new UnityEvent());

        private UnityEvent _onCompleted;
    }

    public abstract class WaiterEntity<T> : WaiterEntity, IWaiterEntity<T>
    {
        public abstract T Result { get; }

        public UnityEvent<T> OnCompletedResult => _onCompletedResult ?? (_onCompletedResult = new UnityEvent<T>());
        
        private UnityEvent<T> _onCompletedResult;
    }

    public interface IWaiterEntity : IEnumerator
    {
        bool IsDone { get; }
        bool Success { get; }
        
        UnityEvent OnCompleted { get; }
    }

    public interface IWaiterEntity<T> : IWaiterEntity
    {
        T Result { get; }

        UnityEvent<T> OnCompletedResult { get; }
    }
}