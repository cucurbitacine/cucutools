using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace cucu.tools
{
    public class CucuTagManager : MonoBehaviour
    {
        [SerializeField] private readonly string search = "";
        [SerializeField] private ObjectsOfTag[] _objectsOfTag;
        private TagStorage _storage;
        [SerializeField] private TagsOfObject[] _tagsOfObjects;

        static CucuTagManager()
        {
            Instance = Init();
        }

        public static CucuTagManager Instance { get; }

        private TagStorage Storage
        {
            get
            {
                if (_storage != null) return _storage;
                _storage = new TagStorage();
                return _storage;
            }
        }

        [ContextMenu("Search")]
        private void Search()
        {
            var tags = search.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
        }

        private static CucuTagManager Init()
        {
            var iam = new GameObject("CucuTagManager");
            DontDestroyOnLoad(iam);
            var instance = iam.AddComponent<CucuTagManager>();

            return instance;
        }

        public void AddTag(CucuTag cucuTag)
        {
            Storage.Add(cucuTag);
        }

        public void EditTag(CucuTag cucuTag)
        {
            if (Storage.Remove(cucuTag)) Storage.Add(cucuTag);
        }

        public void RemoveTag(CucuTag cucuTag)
        {
            Storage.Remove(cucuTag);
        }

        public GameObject[] GetObjectsByTags(params string[] keys)
        {
            return keys.SelectMany(key => Storage.GetObjects(key)).ToArray();
        }

        public CucuTag[] GetTagsByObjects(params GameObject[] objects)
        {
            return objects.SelectMany(o => Storage.GetTags(o).SelectMany(s => Storage.GetTags(s))).ToArray();
        }

        private void UpdateData()
        {
            _objectsOfTag = Storage.GetTags()
                .Select(t => new ObjectsOfTag
                    {key = t, gameObjects = Storage.GetTags(t).Select(s => s.gameObject).ToArray()})
                .ToArray();

            _tagsOfObjects = Storage.GetObjects()
                .Select(go => new TagsOfObject {gameObject = go, tags = Storage.GetTags(go)})
                .ToArray();
        }

        private void Awake()
        {
            Storage.OnChanged.AddListener(UpdateData);
        }

        private void Start()
        {
            UpdateData();
        }

        private void OnDestroy()
        {
            foreach (var tagKey in Storage.GetTags())
            foreach (var cucuTag in Storage.GetTags(tagKey))
                cucuTag.OnChanged.RemoveListener(() => EditTag(cucuTag));
        }

        [Serializable]
        private struct ObjectsOfTag
        {
            public string key;
            public GameObject[] gameObjects;
        }

        [Serializable]
        private struct TagsOfObject
        {
            public GameObject gameObject;
            public string[] tags;
        }

        private class TagStorage
        {
            private readonly Dictionary<int, KeyValuePair<GameObject, List<string>>> _storage_object2tags;

            private readonly Dictionary<string, List<CucuTag>> _storage_tag2objects;

            private readonly Dictionary<GUID, string> _storage_tag2tag;

            public readonly CucuEvent OnChanged = new CucuEvent();

            public TagStorage()
            {
                _storage_tag2objects = new Dictionary<string, List<CucuTag>>();

                _storage_object2tags = new Dictionary<int, KeyValuePair<GameObject, List<string>>>();

                _storage_tag2tag = new Dictionary<GUID, string>();
            }

            public bool Add(CucuTag cucuTag)
            {
                var guid = cucuTag.Guid;

                //

                if (_storage_tag2tag.ContainsKey(guid))
                    return false;

                _storage_tag2tag.Add(guid, cucuTag.Key);

                if (!_storage_tag2tag.TryGetValue(guid, out var key))
                    return false;

                //

                if (!_storage_tag2objects.ContainsKey(key))
                    _storage_tag2objects.Add(key, new List<CucuTag>());

                if (!_storage_tag2objects.TryGetValue(key, out var cucuTags))
                    return false;

                cucuTags.Add(cucuTag);

                //

                var go = cucuTag.gameObject;
                var id = go.GetInstanceID();

                if (!_storage_object2tags.ContainsKey(id))
                    _storage_object2tags.Add(id, new KeyValuePair<GameObject, List<string>>(go, new List<string>()));

                if (!_storage_object2tags.TryGetValue(id, out var pair))
                    return false;

                if (!pair.Value.Contains(key))
                    pair.Value.Add(key);

                //

                OnChanged.Invoke();
                return true;
            }

            public bool Remove(CucuTag cucuTag)
            {
                return _storage_tag2tag.TryGetValue(cucuTag.Guid, out var key) && RemoveFrom(key, cucuTag);
            }

            private bool RemoveFrom(string key, CucuTag cucuTag)
            {
                var guid = cucuTag.Guid;

                //

                _storage_tag2tag.Remove(guid);

                //

                if (_storage_tag2objects.TryGetValue(key, out var cucuTags))
                {
                    cucuTags.Remove(cucuTag);

                    if (!cucuTags.Any()) _storage_tag2objects.Remove(key);
                }

                //

                var id = cucuTag.gameObject.GetInstanceID();

                if (_storage_object2tags.TryGetValue(id, out var pair))
                {
                    pair.Value.Remove(key);

                    if (!pair.Value.Any()) _storage_object2tags.Remove(id);
                }

                //

                OnChanged.Invoke();
                return true;
            }

            public string[] GetTags()
            {
                return _storage_tag2objects.Keys.ToArray();
            }

            public CucuTag[] GetTags(string key)
            {
                return _storage_tag2objects[key].ToArray();
            }

            public CucuTag GetTag(string key)
            {
                return GetTags(key).FirstOrDefault();
            }

            public GameObject[] GetObjects()
            {
                return _storage_object2tags.Select(t => t.Value.Key).ToArray();
            }

            public GameObject[] GetObjects(string key)
            {
                return _storage_tag2objects[key].Select(c => c.gameObject).ToArray();
            }

            public string[] GetTags(GameObject gameObject)
            {
                var id = gameObject.GetInstanceID();
                return _storage_object2tags.TryGetValue(id, out var pair) ? pair.Value.ToArray() : new string[0];
            }
        }
    }
}