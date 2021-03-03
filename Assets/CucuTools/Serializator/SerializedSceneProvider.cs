using UnityEngine;

namespace CucuTools
{
    public abstract class SerializedSceneProvider : MonoBehaviour, ISerializedSceneProvider
    {
        public abstract void CreateScene(string sceneName, params SerializedComponent[] components);

        public abstract SerializedComponent[] ReadScene(string sceneName);

        public abstract void UpdateScene(string sceneName, params SerializedComponent[] components);

        public abstract void DeleteScenes(params string[] sceneNames);
    }
}