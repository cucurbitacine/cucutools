using System.Collections;
using System.Threading.Tasks;
using CucuTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Example.Scripts
{
    public class ExampleController : MonoBehaviour
    {
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
            new WaiterOperation(SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name))
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

        public CucuTimer timer;
        public AnimationBehaviour animation;
        
        private void Start()
        {
            timer = new CucuTimer(3f);

            timer
                .Before(() => Debug.Log($"The animation will start in {timer.Delay} seconds"))
                .After(() => animation.StartAnimation())
                .Start();
        }
    }
}