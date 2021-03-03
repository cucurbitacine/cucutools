using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CucuTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Example.Scripts
{
    public class ExampleController : CucuInjectorBehaviour
    {
        [Header("Example")]
        [SerializeField] private CucuTimer timer;
        [SerializeField] private AnimationBehaviour animationBehaviour;

        [Header("Serialized storage")]
        [SerializeField] private SerializedSceneObject storage;
        
        [InjectArg]
        [SerializeField] private ExampleCucuArg cucuArg;


        #region Log

        public void Log(string msg)
        {
            Debug.Log(msg);
        }

        public void LogWarning(string msg)
        {
            Debug.LogWarning(msg);
        }

        public void LogError(string msg)
        {
            Debug.LogError(msg);
        }

        #endregion

        #region Task

        [CucuButton("Wait Task", group: "Task")]
        private void WaitTask()
        {
            this.Wait(Task()).OnCompleted.AddListener(() => Log("Wait :: Task :: Done"));
        }

        [CucuButton("Wait Task<string>", group: "Task")]
        private void WaitTaskString()
        {
            this.Wait(TaskString()).OnCompletedResult.AddListener(t => Log($"Wait :: Task<string> :: Done :: {t}"));
        }

        private async Task Task()
        {
            await System.Threading.Tasks.Task.Delay(Random.Range(1000, 5000));
        }

        private async Task<string> TaskString()
        {
            await System.Threading.Tasks.Task.Delay(Random.Range(1000, 5000));

            return "Result";
        }

        #endregion

        #region Coroutine

        [CucuButton("Wait coroutine", group: "Coroutine")]
        private void WaitCoroutine()
        {
            this.Wait(Coroutine()).OnCompleted.AddListener(() => Log("Wait :: Coroutine :: Done"));
        }

        private IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
        }

        #endregion

        #region AsyncOperation

        [CucuButton("Wait AsyncOperation", group: "AsyncOperation")]
        private void WaitAsyncOperation()
        {
            var args = new List<CucuArg>();
            
            args.Add(new ExampleCucuArg(3f));

            new WaiterOperation(CucuSceneManager.LoadSingleSceneAsync(
                    SceneManager.GetActiveScene().name, args.ToArray()))
                .OnCompleted.AddListener(() => Debug.Log("Wait :: AsyncOperation :: Done"));
        }

        #endregion

        #region Custom

        [CucuButton("Wait custom", group: "Custom")]
        private void WaitCustom()
        {
            var waiter = new WaiterCustom();
            waiter.OnCompleted.AddListener(() => Log($"Wait :: Custom :: Done"));

            StartCoroutine(InvokeDelay(waiter, Random.Range(1f, 5f)));
        }

        [CucuButton("Wait custom result", group: "Custom")]
        private void WaitCustomString()
        {
            var waiter = new WaiterCustom<string>();
            waiter.OnCompletedResult.AddListener(t => Log($"Wait :: Custom :: Done :: {t}"));

            StartCoroutine(InvokeDelay(waiter, Random.Range(1f, 5f)));
        }

        private IEnumerator InvokeDelay(WaiterCustom custom, float delay)
        {
            yield return new WaitForSeconds(delay);

            custom.Invoke();
        }

        private IEnumerator InvokeDelay(WaiterCustom<string> custom, float delay)
        {
            yield return new WaitForSeconds(delay);

            custom.Invoke("Result");
        }

        #endregion

        protected override void OnAwake()
        {
            timer = new CucuTimer(0f);

            if (cucuArg != null && cucuArg.IsValid) timer.Delay = cucuArg.delay;

            timer
                .Before(() => Debug.Log($"Starting animation in {timer.Delay} seconds"))
                .After(() => animationBehaviour.StartAnimation())
                .Start();
        }
    }

    [Serializable]
    public class ExampleCucuArg : CucuArg
    {
        [Min(0f)] public float delay;

        public ExampleCucuArg(float d)
        {
            delay = d;
        }
    }
}