using System.Collections;
using System.Collections.Generic;
using cucu.tools;
using UnityEngine;

namespace cucu.example
{
    public class CucuExampleSceneController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                var go = new GameObject($"#{Random.Range(1000, 9999)}");
                go.AddCucuTag("npc");
            }
        }
    }

}