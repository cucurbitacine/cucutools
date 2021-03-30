using System.Threading.Tasks;
using CucuTools.Serializing.Components;
using UnityEngine;

namespace CucuTools.Serializing
{
    public abstract class SerializedSceneProvider : MonoBehaviour, ISerializedSceneProvider
    {
        public abstract Task CreateScene(string sceneDataName, params SerializedComponent[] components);

        public abstract Task<SerializedComponent[]> ReadScene(string sceneDataName);

        public abstract Task UpdateScene(string sceneDataName, params SerializedComponent[] components);

        public abstract Task DeleteScenes(params string[] sceneDataNames);
    }
}