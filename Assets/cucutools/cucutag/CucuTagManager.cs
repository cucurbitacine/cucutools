using System;
using System.Linq;
using UnityEngine;

namespace cucu.tools
{
    public class CucuTagManager : MonoBehaviour
    {
        public static CucuTagManager Instance { get; }
#if UNITY_EDITOR
        [SerializeField] private bool UpdateViewList = true;
        [Header("Online")]
        [SerializeField] private StackTagInfo[] _stackTagsOnline;
        [Space]
        [Header("Offline")]
        [SerializeField] private StackTagInfo[] _stackTagsOffline;

        [Space]
        [Header("Search")]
        [SerializeField] private string search = "";
#endif
        private TagStorage Storage => _storage ?? (_storage = new TagStorage());
        private TagStorage _storage;

        static CucuTagManager()
        {
            Instance = Init();
        }
#if UNITY_EDITOR
        private void Update()
        {
            UpdateData();
        }
#endif
#if UNITY_EDITOR
        [ContextMenu("Search")]
        private void Search()
        {
            var tags = search.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

            var count = tags.Length;

            var all = tags
                .SelectMany(t =>
                {
                    var key = t;
                    var data = Storage.GetData(key);
                    var gO = data.Select(l => l.gameObject);
                    var res = gO.Distinct();
                    return res;
                })
                .ToArray();

            var unique = all
                .Distinct()
                .Select(a => (a.GetInstanceID(), a));

            var result = unique
                .Where(u => all.Count(x => x.GetInstanceID() == u.Item1) == count)
                .Select(u => u.a);

            var info = $"({search}) : [";
            foreach (var r in result)
            {
                info += r.name + ",";
            }

            info += "]";
            Cucu.Log(info, "Searching");
        }
#endif
        private static CucuTagManager Init()
        {
            var iam = new GameObject("CucuTagManager");
            DontDestroyOnLoad(iam);
            var instance = iam.AddComponent<CucuTagManager>();

            return instance;
        }
        
        public bool Register(CucuTag cucuTag)
        {
            return Storage.Create(cucuTag);
        }

        public bool Unregistering(CucuTag cucuTag)
        {
            return Storage.Delete(cucuTag);
        }

        public bool UpdateTag(CucuTag cucuTag)
        {
            return Storage.Update(cucuTag);
        }

        public bool LogIn(CucuTag cucuTag)
        {
            if (!Storage.Data.ContainsKey(cucuTag.Guid)) return false;
            return !cucuTag.IsOnline;

        }

        public bool LogOut(CucuTag cucuTag)
        {
            if (!Storage.Data.ContainsKey(cucuTag.Guid)) return false;
            return cucuTag.IsOnline;
        }

        public bool TagIsOnline(CucuTag cucuTag)
        {
            return cucuTag != null && Storage.Data.ContainsKey(cucuTag.Guid) && cucuTag.IsOnline;
        }

        public GameObject[] FindObjectsByTag(string key)
        {
            var tags = Storage.GetData(key);

            var onlineTags = tags.Where(TagIsOnline);

            return onlineTags.Select(d => d.gameObject).ToArray();
        }
#if UNITY_EDITOR
        [ContextMenu("Refresh")]
        private void UpdateData()
        {
            var allTags = !UpdateViewList
                ? null
                : Storage.StackedData
                    .Select(sd => new StackTagInfo
                    {
                        key = sd.Key,
                        tags = sd.Value.Select(g => Storage.Data[g])
                            .Select(t => new TagInfo
                            {
                                nameObject = t.gameObject.name,
                                online = TagIsOnline(t),
                                gameObjects = t.gameObject,
                            })
                            .ToArray(),
                    });

            _stackTagsOnline = allTags?
                .Select(at => new StackTagInfo {key = at.key, tags = at.tags.Where(ts => ts.online).ToArray()})
                .Where(on => on.tags.Any())?.ToArray();

            _stackTagsOffline = allTags?
                .Select(at => new StackTagInfo { key = at.key, tags = at.tags.Where(ts => !ts.online).ToArray() })
                .Where(on => on.tags.Any()).ToArray();
        }
#endif
        [Serializable]
        private struct StackTagInfo
        {
            public string key;
            public TagInfo[] tags;
        }

        [Serializable]
        private struct TagInfo
        {
            public string nameObject;
            [NonSerialized] public bool online;
            public GameObject gameObjects;
        }
    }
}