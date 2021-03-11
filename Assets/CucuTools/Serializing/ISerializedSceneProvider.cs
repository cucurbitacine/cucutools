using System.Threading.Tasks;
using CucuTools.Serializing.Components;

namespace CucuTools.Serializing
{
    /// <summary>
    /// CRUD for serialized components of scene
    /// </summary>
    public interface ISerializedSceneProvider
    {
        Task CreateScene(string sceneDataName, params SerializedComponent[] components);
        Task<SerializedComponent[]> ReadScene(string sceneDataName);
        Task UpdateScene(string sceneDataName, params SerializedComponent[] components);
        Task DeleteScenes(params string[] sceneDataNames);
    }
}