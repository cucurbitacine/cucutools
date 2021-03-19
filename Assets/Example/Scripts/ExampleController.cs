using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CucuTools.ArgumentInjector;
using CucuTools.Attributes;
using CucuTools.Common;
using CucuTools.Waiters.Impl;
using UnityEngine;

namespace Example.Scripts
{
    [SceneController("cucuexample")]
    public class ExampleController : CucuSceneController
    {
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

        public void ReloadScene(int indexZone)
        {
            var args = new List<CucuArg>();

            args.Add(new ExampleCucuArg(indexZone));

            new WaiterOperation(CucuSceneManager.LoadSingleSceneAsync<ExampleController>(args.ToArray()))
                .OnCompleted.AddListener(() => Debug.Log($"Loaded scene with zone : {indexZone}"));
        }
        
        public void ReloadScene()
        {
            ReloadScene(0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var players = FindObjectsOfType<PlayerController>();
                var player = players.FirstOrDefault(p => p.IsEnabled);

                var anotherPlayer = players.FirstOrDefault(p => p != player);
                if (anotherPlayer != null)
                {
                    player.IsEnabled = false;
                    anotherPlayer.IsEnabled = true;
                }
            }
        }
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