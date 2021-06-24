using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace CucuTools.Async
{
    public static class CucuAsyncExt
    {
        public static async Task<TaskResult> WithTimeOut(this Task task, int milliseconds)
        {
            return new TaskResult(task, await Task.WhenAny(task, Task.Delay(milliseconds)) != task, milliseconds);
        }

        public static async Task<TaskResult<T>> WithTimeOut<T>(this Task<T> task, int milliseconds)
        {
            return new TaskResult<T>(task, await Task.WhenAny(task, Task.Delay(milliseconds)) != task, milliseconds);
        }
        
        public static async Task ToTask(this IEnumerator enumerator, MonoBehaviour root)
        {
            var taskSource = new TaskCompletionSource<object>();

            root.StartCoroutine(Coroutine(taskSource, enumerator));

            await taskSource.Task;
        }
    
        public static async Task<T> ToTask<T>(this IEnumerator<T> enumerator, MonoBehaviour root)
        {
            var taskSource = new TaskCompletionSource<T>();

            root.StartCoroutine(Coroutine(taskSource, enumerator));

            return await taskSource.Task;
        }
    
        public static async Task ToTask(this IEnumerator enumerator)
        {
            await enumerator.ToTask(CucuCoroutine.Root);
        }
    
        public static async Task<T> ToTask<T>(this IEnumerator<T> enumerator)
        {
            return await enumerator.ToTask(CucuCoroutine.Root);
        }

        public static async Task ToTask(this Coroutine coroutine)
        {
            var taskSource = new TaskCompletionSource<object>();

            CucuCoroutine.Start(Coroutine(taskSource, coroutine));

            await taskSource.Task;
        }
        
        public static async Task<T> ToTask<T>(this UnityEvent<T> unityEvent)
        {
            var tcs = new TaskCompletionSource<T>();

            unityEvent.AddListener(t => tcs.TrySetResult(t));
        
            return await tcs.Task;
        }
    
        public static async Task ToTask(this UnityEvent unityEvent)
        {
            var tcs = new TaskCompletionSource<object>();

            unityEvent.AddListener(() => tcs.TrySetResult(default));

            await tcs.Task;
        }

        private static IEnumerator Coroutine<T>(TaskCompletionSource<T> tcs, IEnumerator enumerator)
        {
            while (enumerator.MoveNext()) yield return enumerator.Current;
            tcs.TrySetResult(default);
        }
    
        private static IEnumerator Coroutine<T>(TaskCompletionSource<T> tcs, IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext()) yield return enumerator.Current;
            tcs.TrySetResult(enumerator.Current);
        }
        
        private static IEnumerator Coroutine<T>(TaskCompletionSource<T> tcs, Coroutine coroutine)
        {
            yield return coroutine;
            tcs.TrySetResult(default);
        }
    }
}