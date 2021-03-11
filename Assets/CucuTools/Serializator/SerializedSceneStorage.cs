using System.Threading.Tasks;

namespace CucuTools
{
    public class SerializedSceneStorage : SerializedSceneProvider
    {
        public SerializedSceneObject storage;
        
        public override async Task CreateScene(string sceneName, params SerializedComponent[] components)
        {
            await storage.CreateScene(sceneName, components);
        }

        public override async Task<SerializedComponent[]> ReadScene(string sceneName)
        {
            return await storage.ReadScene(sceneName);
        }

        public override async Task UpdateScene(string sceneName, params SerializedComponent[] components)
        {
            await storage.UpdateScene(sceneName, components);
        }

        public override async Task DeleteScenes(params string[] sceneNames)
        {
            await storage.DeleteScenes(sceneNames);
        }
    }
}