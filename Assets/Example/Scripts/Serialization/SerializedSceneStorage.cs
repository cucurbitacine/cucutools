using System.Threading.Tasks;
using CucuTools.Serializing;
using CucuTools.Serializing.Components;

namespace Example.Scripts.Serialization
{
    public class SerializedSceneStorage : SerializedSceneProvider
    {
        public SerializedSceneObject storage;
        
        public override async Task CreateScene(string sceneDataName, params SerializedComponent[] components)
        {
            await storage.CreateScene(sceneDataName, components);
        }

        public override async Task<SerializedComponent[]> ReadScene(string sceneDataName)
        {
            return await storage.ReadScene(sceneDataName);
        }

        public override async Task UpdateScene(string sceneDataName, params SerializedComponent[] components)
        {
            await storage.UpdateScene(sceneDataName, components);
        }

        public override async Task DeleteScenes(params string[] sceneDataNames)
        {
            await storage.DeleteScenes(sceneDataNames);
        }
    }
}