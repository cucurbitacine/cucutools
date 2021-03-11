using System;
using UnityEngine;

namespace CucuTools.Waiters.Impl
{
    [Serializable]
    public class WaiterCustom : WaiterEntity
    {
        public override bool IsDone => isDone;
        public override bool Success => success;

        [Header("Waiter")]
        [SerializeField] private bool isDone;
        [SerializeField] private bool success;

        public WaiterCustom()
        {
            isDone = false;
            success = false;
        }

        public virtual void Invoke(bool status = true)
        {
            if (isDone) return;

            isDone = true;
            success = status;

            OnCompleted.Invoke();
        }
    }

    [Serializable]
    public class WaiterCustom<T> : WaiterEntity<T>
    {
        public override bool IsDone => isDone;
        public override bool Success => success;

        public override T Result => result;

        [Header("Waiter")]
        [SerializeField] private bool isDone;
        [SerializeField] private bool success;
        [SerializeField] private T result;

        public WaiterCustom(T result)
        {
            this.result = result;

            isDone = false;
            success = false;
        }

        public WaiterCustom() : this(default)
        {
        }

        public virtual void Invoke(bool status = true)
        {
            if (isDone) return;

            isDone = true;
            success = status;

            try
            {
                OnCompleted.Invoke();
            }
            catch
            {
            }

            try
            {
                OnCompletedResult.Invoke(Result);
            }
            catch
            {
            }
        }

        public virtual void Invoke(T result, bool status = true)
        {
            if (isDone) return;

            this.result = result;

            Invoke(status);
        }
    }
}