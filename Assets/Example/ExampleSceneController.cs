using System;
using CucuTools;
using CucuTools.Injects;
using CucuTools.Scenes;
using CucuTools.Workflows;
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

        protected override IContainer Container =>
            (containerArg?.IsDefault ?? true) ? GetDefaultContainer() : containerArg.container; 
        
        [Header("Info")]
        public string login = "admin";
        public string password = "1234";

        [Header("Settings")]
        [CucuArg] public UserArg user;
        [CucuArg] public ContainerArg containerArg;

        [Header("Events")]
        public UnityEvent OnStart;
        
        [Header("References")]
        public SceneLoader sceneLoader;
        
        public SwitchBoolBehaviour haveUser;
        
        public ConditionEntity validLogin;
        public ConditionEntity validPassword;
        public ConditionEntity wrongUser;
        
        public Text loginSource;
        public Text passwordSource;

        [CucuInject]
        private ILogger _logger = null;
        
        public void CheckUser()
        {
            haveUser.Value = user != null && !user.IsDefault;

            _logger.Log($"User {(haveUser.Value ? "" : "doesn't ")}found");
        }

        public void VerifyUser()
        {
            validLogin.Done = user.login == login;
            validPassword.Done = user.password == password;
            wrongUser.Done = !validPassword.Done || !validLogin.Done;

            _logger.Log($"Login {(validLogin.Done ? "" : "doesn't ")}pass");
            _logger.Log($"Password {(validPassword.Done ? "" : "doesn't ")}pass");
        }
        
        public void ReloadWithUser()
        {
            sceneLoader.AddArgs(user);
            sceneLoader.AddArgs(new ContainerArg() {container = CucuDI.Instance.GetContainer()});
            sceneLoader.LoadSingleScene();

            _logger.Log($"Reload scene with user \"{user.login}\"");
        }

        public void ReloadWithoutUser()
        {
            sceneLoader.ClearArgs();
            sceneLoader.LoadSingleScene();
            
            _logger.Log($"Reload scene without user");
        }

        private IContainer GetDefaultContainer()
        {
            var container = new CucuContainer();
            container.Bind<ILogger>().ToSingleton<EmptyLogger>();
            return container;
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
    
    [Serializable]
    public class ContainerArg : CucuArg
    {
        public IContainer container;
    }
}