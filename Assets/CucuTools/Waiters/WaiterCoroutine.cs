using System;
using System.Collections;
using UnityEngine;

namespace CucuTools
{
    [Serializable]
    public class WaiterCoroutine : WaiterEntity
    {
        public override bool IsDone => isDone;
        public override bool Success => success;

        public MonoBehaviour Root
        {
            get => root;
            protected set => root = value;
        }

        [Header("Waiter info")]
        [SerializeField] protected bool isDone;
        [SerializeField] protected bool success;
        
        [Header("Coroutine settings")]
        [SerializeField] private MonoBehaviour root;

        protected Coroutine _coroutine;
        protected Coroutine _coroutineInternal;


        public WaiterCoroutine(MonoBehaviour root, IEnumerator enumerator)
        {
            Root = root;

            Start(enumerator);
        }

        private void Start(IEnumerator enumerator)
        {
            if (_coroutine != null) Stop();

            _coroutine = Root.StartCoroutine(StartCoroutine(enumerator));
        }

        private void Stop()
        {
            if (_coroutine == null) return;

            Root.StopCoroutine(_coroutine);

            isDone = true;
            success = false;
            _coroutine = null;

            OnCompleted.Invoke();
        }

        private IEnumerator StartCoroutine(IEnumerator enumerator)
        {
            isDone = false;
            success = false;
            
            _coroutineInternal = Root.StartCoroutine(enumerator);

            yield return _coroutineInternal;

            isDone = true;

            _coroutine = null;

            OnCompleted.Invoke();
        }
    }

    public static class WaiterCoroutineExt
    {
        public static WaiterCoroutine Wait(this MonoBehaviour root, IEnumerator enumerator)
        {
            return new WaiterCoroutine(root, enumerator);
        }
    }
}