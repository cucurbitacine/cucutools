using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace CucuTools.Waiters.Impl
{
    public class WaiterTask : WaiterEntity, IDisposable
    {
        public override bool IsDone => Task.IsCompleted;
        public override bool Success => Task.IsCompleted && !Task.IsCanceled && !Task.IsFaulted;

        public Task Task { get; }

        public MonoBehaviour Root { get; }

        public WaiterTask(MonoBehaviour root, Task task)
        {
            Root = root;
            Task = task;

            Root.StartCoroutine(StartCoroutine());
        }

        private IEnumerator StartCoroutine()
        {
            yield return this;

            OnCompleted.Invoke();
        }

        public void Dispose()
        {
            Task?.Dispose();
        }
    }
    
    public class WaiterTask<T> : WaiterEntity<T>, IDisposable
    {
        public override bool IsDone => Task.IsCompleted;
        public override bool Success => Task.IsCompleted && !Task.IsCanceled && !Task.IsFaulted;
        public override T Result => Task.Result;

        public Task<T> Task { get; }

        public MonoBehaviour Root { get; }

        public WaiterTask(MonoBehaviour root, Task<T> task)
        {
            Root = root;
            Task = task;

            Root.StartCoroutine(StartCoroutine());
        }

        private IEnumerator StartCoroutine()
        {
            yield return this;

            OnCompleted.Invoke();

            OnCompletedResult.Invoke(Result);
        }

        public void Dispose()
        {
            Task?.Dispose();
        }
    }

    public static class WaiterTaskExt
    {
        public static WaiterTask Wait(this MonoBehaviour root, Task task)
        {
            return new WaiterTask(root, task);
        }
        
        public static WaiterTask<T> Wait<T>(this MonoBehaviour root, Task<T> task)
        {
            return new WaiterTask<T>(root, task);
        }
    }
}