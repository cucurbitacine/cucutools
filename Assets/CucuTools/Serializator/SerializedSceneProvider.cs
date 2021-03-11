using System.Threading.Tasks;
using UnityEngine;

namespace CucuTools
{
    public abstract class SerializedSceneProvider : MonoBehaviour, ISerializedSceneProvider
    {
        public abstract Task CreateScene(string sceneName, params SerializedComponent[] components);

        public abstract Task<SerializedComponent[]> ReadScene(string sceneName);

        public abstract Task UpdateScene(string sceneName, params SerializedComponent[] components);

        public abstract Task DeleteScenes(params string[] sceneNames);
    }
}