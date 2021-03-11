using System;
using UnityEngine;

namespace CucuTools.Waiters.Impl
{
    [Serializable]
    public class WaiterOperation : WaiterEntity
    {
        public override bool IsDone => Operation?.isDone ?? false;
        public override bool Success => IsDone;
        public float Progress => Operation?.progress ?? 0f;

        public AsyncOperation Operation { get; }

        public WaiterOperation(AsyncOperation operation)
        {
            Operation = operation;

            Operation.completed += ao => { OnCompleted.Invoke(); };
        }
    }
}