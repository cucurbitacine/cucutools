namespace CucuTools
{
    public class SerializedSceneStorage : SerializedSceneProvider
    {
        public SerializedSceneObject storage;
        
        public override void CreateScene(string sceneName, params SerializedComponent[] components)
        {
            storage.CreateScene(sceneName, components);
        }

        public override SerializedComponent[] ReadScene(string sceneName)
        {
            return storage.ReadScene(sceneName);
        }

        public override void UpdateScene(string sceneName, params SerializedComponent[] components)
        {
            storage.UpdateScene(sceneName, components);
        }

        public override void DeleteScenes(params string[] sceneNames)
        {
            storage.DeleteScenes(sceneNames);
        }
    }
}