using System.Collections;
using System.Collections.Generic;
using cucu.tools;
using UnityEngine;

namespace cucu.example
{
    public class ExampleCucuLog : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            //var log = new CucuLog();
            var logError = new CucuLog.LogParams(CucuLog.LogParams.LogType.Error, CucuLog.LogParams.LogArea.Anywhere);
            var failTag = new CucuLog.TagParams(CucuLog.TagParams.TagType.Fail, "FAILED");
            CucuLog.Log("Тестовая ошибка", logError);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}