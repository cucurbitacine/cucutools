using System;
using System.Linq;
using System.Threading.Tasks;
using CucuTools.FileUtility;
using CucuTools.Serializing;
using CucuTools.Serializing.Components;
using UnityEngine;

namespace Example.Scripts.Serialization
{
    public class SerializedSceneFile : SerializedSceneProvider
    {
        public DirectoryUnit DirectoryUnit => _directoryUnit ?? (_directoryUnit = new DirectoryUnit(folderName));
        
        [SerializeField] private string folderName;
        [SerializeField] private string fileExtention = "json";
        
        private DirectoryUnit _directoryUnit;
        
        public override async Task CreateScene(string sceneDataName, params SerializedComponent[] components)
        {
            var fileName = GetSceneFileName(sceneDataName);
            
            if (!DirectoryUnit.Exists()) DirectoryUnit.CreateDirectory();
            
            var json = GetJson(components);
            
            await DirectoryUnit.CreateFileAsync(fileName, json);
        }

        public override async Task<SerializedComponent[]> ReadScene(string sceneDataName)
        {
            var fileName = GetSceneFileName(sceneDataName);
            
            if (!DirectoryUnit.ExistsFile(fileName)) return new SerializedComponent[0];

            var json = await DirectoryUnit.ReadFileStringAsync(fileName);

            return JsonUtility.FromJson<ComponentDTO>(json).components;
        }

        public override async Task UpdateScene(string sceneDataName, params SerializedComponent[] components)
        {
            var fileName = GetSceneFileName(sceneDataName);
            
            var prevComps = await ReadScene(fileName);

            var newComps = components.Where(c => prevComps.All(p => p.Guid != c.Guid)).ToArray();
            var oldComps = components.Where(c => prevComps.Any(p => p.Guid == c.Guid)).ToArray();

            for (var i = 0; i < prevComps.Length; i++)
            {
                var comp = prevComps[i];

                var old = oldComps.FirstOrDefault(o => o.Guid == comp.Guid);

                if (old == null) continue;

                comp.bytes = old.bytes;
            }

            var comps = prevComps.Concat(newComps).ToArray();
            
            var json = GetJson(comps);

            await DirectoryUnit.WriteFileAsync(fileName, json);
        }

        public override Task DeleteScenes(params string[] sceneDataNames)
        {
            if (!DirectoryUnit.Exists()) return Task.CompletedTask;

            foreach (var sceneDataName in sceneDataNames)
            {
                var fileName = GetSceneFileName(sceneDataName);
                
                if (!DirectoryUnit.ExistsFile(fileName)) continue;
                
                DirectoryUnit.DeleteFile(fileName);
            }
            
            return Task.CompletedTask;
        }

        private string GetJson(params SerializedComponent[] components)
        {
            return JsonUtility.ToJson(new ComponentDTO(components));
        }

        private string GetSceneFileName(string sceneDataName)
        {
            return sceneDataName.FileExt(fileExtention);
        }
        
        private void OnValidate()
        {
            folderName = Application.streamingAssetsPath;
            DirectoryUnit.DirectoryPath = folderName;
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