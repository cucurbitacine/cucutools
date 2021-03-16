
using System;

namespace CucuTools.Common
{
    public abstract class CucuSceneController : CucuMonoBehaviour
    {
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SceneControllerAttribute : Attribute
    {
        public string SceneName { get; }
        
        public SceneControllerAttribute(string sceneName)
        {
            SceneName = sceneName;
        }
    }
}