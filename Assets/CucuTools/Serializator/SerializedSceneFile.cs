using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Directory = System.IO.Directory;
using File = System.IO.File;

namespace CucuTools
{
    public class SerializedSceneFile : SerializedSceneProvider
    {
        private Encoding Encoding => Encoding.Default;
        
        [SerializeField] private string folderName;

        public override async Task CreateScene(string sceneName, params SerializedComponent[] components)
        {
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            
            using var fs = new FileStream(GetPath(sceneName), FileMode.Create);
            
            var json = GetJson(components);

            var bytes = Encoding.GetBytes(json);

            await fs.WriteAsync(bytes, 0, bytes.Length);
        }

        public override async Task<SerializedComponent[]> ReadScene(string sceneName)
        {
            if (!Directory.Exists(folderName)) return new SerializedComponent[0];

            if (!File.Exists(GetPath(sceneName))) return new SerializedComponent[0];
            
            using var fs = new FileStream(GetPath(sceneName), FileMode.Open);

            var bytes = new byte[fs.Length];

            await fs.ReadAsync(bytes, 0, (int) fs.Length);

            var json = Encoding.GetString(bytes);

            return JsonUtility.FromJson<ComponentDTO>(json).components;
        }

        public override async Task UpdateScene(string sceneName, params SerializedComponent[] components)
        {
            if (!Directory.Exists(folderName) || !File.Exists(GetPath(sceneName)))
            {
                await CreateScene(sceneName, components);
                return;
            }

            var prevComps = await ReadScene(sceneName);

            var newComps = components.Where(c => prevComps.All(p => p.Guid != c.Guid)).ToArray();
            var oldComps = components.Where(c => prevComps.Any(p => p.Guid == c.Guid)).ToArray();

            for (var i = 0; i < prevComps.Length; i++)
            {
                var comp = prevComps[i];

                var old = oldComps.FirstOrDefault(o => o.Guid == comp.Guid);

                if (old == null) continue;

                comp.serializedData = old.serializedData;
            }

            var comps = prevComps.Concat(newComps).ToArray();
            
            using var fs = new FileStream(GetPath(sceneName), FileMode.Create);
            
            var json = GetJson(comps);

            var bytes = Encoding.GetBytes(json);

            await fs.WriteAsync(bytes, 0, bytes.Length);
        }

        public override Task DeleteScenes(params string[] sceneNames)
        {
            if (!Directory.Exists(folderName)) return Task.CompletedTask;

            foreach (var sceneName in sceneNames)
            {
                if (!File.Exists(GetPath(sceneName))) return Task.CompletedTask;
                
                File.Delete(GetPath(sceneName));
            }
            
            return Task.CompletedTask;
        }

        private string GetFileName(string sceneName)
        {
            return $"{sceneName}.json";
        }

        private string GetPath(string sceneName)
        {
            return Path.Combine(folderName, GetFileName(sceneName));
        }

        private string GetJson(params SerializedComponent[] components)
        {
            return JsonUtility.ToJson(new ComponentDTO(components));
        }
        
        private void OnValidate()
        {
            folderName = Application.streamingAssetsPath;
        }

        [Serializable]
        private class ComponentDTO
        {
            public SerializedComponent[] components;

            public ComponentDTO(params SerializedComponent[] components)
            {
                this.components = components;
            }
        }
    }
}