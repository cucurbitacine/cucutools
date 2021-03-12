using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using CucuTools.ArgumentInjector;
using CucuTools.Attributes;
using CucuTools.Common;
using CucuTools.Waiters.Impl;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Example.Scripts
{
    public class ExampleController : CucuMonoBehaviour
    {
        [CucuArg]
        public ExampleCucuArg exampleCucuArg;
        
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

        [CucuButton("Wait Task", group: "Waiter", colorHex: "0000AA")]
        private void WaitTask()
        {
            this.Wait(Task()).OnCompleted.AddListener(() => Log("Wait :: Task :: Done"));
        }

        [CucuButton("Wait Task<string>", group: "Waiter", colorHex: "0000BB")]
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

        [CucuButton("Wait coroutine", group: "Waiter", colorHex: "00AA00")]
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

        [CucuButton("Wait AsyncOperation", group: "Waiter", colorHex: "AA0000")]
        public void WaitAsyncOperation()
        {
            var args = new List<CucuArg>();

            var indexZone = exampleCucuArg?.indexZone ?? 0;

            indexZone = (indexZone + 1) % FindObjectsOfType<ZoneExampleController>().Length;

            args.Add(new ExampleCucuArg(indexZone));

            new WaiterOperation(CucuSceneManager.LoadSingleSceneAsync(
                    SceneManager.GetActiveScene().name, args.ToArray()))
                .OnCompleted.AddListener(() => Debug.Log("Wait :: AsyncOperation :: Done"));
        }

        #endregion

        #region Custom

        [CucuButton("Wait custom", group: "Waiter", colorHex: "AAAA00")]
        private void WaitCustom()
        {
            var waiter = new WaiterCustom();
            waiter.OnCompleted.AddListener(() => Log($"Wait :: Custom :: Done"));

            StartCoroutine(InvokeDelay(waiter, Random.Range(1f, 5f)));
        }

        [CucuButton("Wait custom result", group: "Waiter", colorHex: "BBBB00")]
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
    }

    [Serializable]
    public class ExampleCucuArg : CucuArg
    {
        public int indexZone;

        public ExampleCucuArg(int index)
        {
            indexZone = index;
        }
    }
}