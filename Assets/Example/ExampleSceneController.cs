using System;
using CucuTools;
using CucuTools.Injects;
using CucuTools.Scenes;
using CucuTools.Workflows.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Example
{
    [CucuSceneController(ExampleSceneController.SceneName)]
    public class ExampleSceneController : CucuSceneController
    {
        public const string SceneName = "Example";
        private const string loadGroup = "Load Scene...";
        
        [Header("Info")]
        public string login = "admin";
        public string password = "1234";
        
        [Header("Settings")]
        [CucuArg]
        public UserArg user;

        [Header("Events")]
        public UnityEvent OnStart;
        
        [Header("References")]
        public SceneLoader sceneLoader;
        
        public ConditionEntity noUser;
        public ConditionEntity haveUser;
        
        public ConditionEntity validUser;
        public ConditionEntity wrongUser;
        
        public Text loginSource;
        public Text passwordSource;
        
        public void CheckUser()
        {
            haveUser.Done = user != null && !user.IsDefault;
            noUser.Done = !haveUser.Done;
        }

        public void VerifyUser()
        {
            validUser.Done = user.login == login && user.password == password;
            wrongUser.Done = !validUser.Done;
        }
        
        public void ReloadWithUser()
        {
            sceneLoader.AddArgs(user);
            sceneLoader.LoadSingleScene();
        }

        public void ReloadWithoutUser()
        {
            sceneLoader.ClearArgs();
            sceneLoader.LoadSingleScene();
        }
        
        
        private void Start()
        {
            OnStart.Invoke();
        }

        private void Update()
        {
            user.login = loginSource.text;
            user.password = passwordSource.text;
        }
    }

    [Serializable]
    public class UserArg : CucuArg
    {
        public string login;
        public string password;
    }
}