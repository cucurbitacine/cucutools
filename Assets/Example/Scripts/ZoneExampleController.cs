using CucuTools.ArgumentInjector;
using CucuTools.Attributes;
using CucuTools.Common;
using UnityEngine;
using UnityEngine.Events;

namespace Example.Scripts
{
    public class ZoneExampleController : CucuMonoBehaviour
    {
        public int indexZone;
        
        [CucuArg]
        public ExampleCucuArg exampleCucuArg;

        public Transform spawnPoint;

        public UnityEvent OnSpawn;

        public void Spawn()
        {
            var player = FindObjectOfType<PlayerController>();

            if (player == null) return;

            player.transform.Set(spawnPoint);

            (OnSpawn ?? (OnSpawn = new UnityEvent())).Invoke();
        }
        
        protected override void OnAwake()
        {
            base.OnAwake();
            
            if (indexZone == (exampleCucuArg?.indexZone ?? 0)) Spawn();
        }
    }
}
