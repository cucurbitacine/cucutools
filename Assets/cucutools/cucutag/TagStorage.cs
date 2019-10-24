using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace cucu.tools
{
    public class TagStorage : IStorageDataStack<string, GUID, CucuTag>
    {
        public IDictionary<string, IList<GUID>> StackedData { get; }

        public IDictionary<GUID, string> Links { get; }

        public IDictionary<GUID, CucuTag> Data { get; }

        public readonly CucuEvent OnUpdated;

        private IDictionary<string, IList<CucuTag>> _hashedStackedData;
        private IList<CucuTag> _hashedData;
        private IList<string> _hashedKeys;
        private bool _isHashedStackedData;
        private bool _isHashedData;
        private bool _isHashedKeys;

        public TagStorage()
        {
            StackedData = new Dictionary<string, IList<GUID>>();
            Links = new Dictionary<GUID, string>();
            Data = new Dictionary<GUID, CucuTag>();

            _hashedStackedData = new Dictionary<string, IList<CucuTag>>();
            _hashedData = new List<CucuTag>();
            _hashedKeys = new List<string>();

            OnUpdated = new CucuEvent();
            OnUpdated.AddListener(() =>
            {
                _isHashedStackedData = false;
                _isHashedData = false;
                _isHashedKeys = false;
            });
        }

        public bool Create(CucuTag tag)
        {
            if (tag == null) return false;

            var guid = tag.Guid;

            if (Data.ContainsKey(guid)) return false;

            Data.Add(guid, tag);

            if (Links.TryGetValue(guid, out var key))
            {
                throw new Exception("!");
            }

            key = tag.Key;
            Links.Add(guid, key);

            if (!StackedData.TryGetValue(key, out var list))
            {
                list = new List<GUID>();
                StackedData.Add(key, list);
            }

            if (list.Contains(guid))
            {
                throw new Exception("!");
            }

            list.Add(guid);

            OnUpdated.Invoke();

            return true;
        }

        public bool Update(CucuTag tag)
        {
            if (tag == null) return false;

            var guid = tag.Guid;

            if (!Data.ContainsKey(guid)) return false;

            if (!Links.TryGetValue(guid, out var key))
            {
                throw new Exception("!");
            }

            var newKey = tag.Key;

            if (key.Equals(newKey)) return false;

            if (!StackedData.TryGetValue(key, out var list))
            {
                throw new Exception("!");
            }

            if (!list.Contains(guid))
            {
                throw new Exception("!");
            }

            list.Remove(guid);

            if (!list.Any()) StackedData.Remove(key);

            Links[guid] = newKey;

            if (!StackedData.TryGetValue(newKey, out list))
            {
                list = new List<GUID>();
                StackedData.Add(newKey, list);
            }

            list.Add(guid);

            OnUpdated.Invoke();

            return true;
        }

        public bool Delete(CucuTag tag)
        {
            if (tag == null) return false;

            var guid = tag.Guid;

            if (!Data.ContainsKey(guid)) return false;

            if (!Links.TryGetValue(guid, out var key))
            {
                throw new Exception("!");
            }

            if (!StackedData.TryGetValue(key, out var list))
            {
                throw new Exception("!");
            }

            if (!list.Contains(guid))
            {
                throw new Exception("!");
            }

            list.Remove(guid);

            if (!list.Any()) StackedData.Remove(key);

            Links.Remove(guid);

            Data.Remove(guid);

            OnUpdated.Invoke();

            return true;
        }

        public IList<CucuTag> GetData(string key)
        {
            if (false /*_isHashedStackedData*/ && _hashedStackedData.TryGetValue(key, out var hash)) return hash;

            hash = StackedData.TryGetValue(key, out var guids)
                ? guids.Select(guid => Data[guid]).ToList()
                : new List<CucuTag>();
            /*
            if (_hashedStackedData.ContainsKey(key)) _hashedStackedData[key] = hash;
            else _hashedStackedData.Add(key, hash);
            */
            _isHashedStackedData = true;

            return hash;
        }

        public IList<CucuTag> GetData()
        {
            if (_isHashedData) return _hashedData;

            _hashedData = Data.Select(data => data.Value).ToList();
            _isHashedData = true;

            return _hashedData;
        }

        public IList<string> GetKeys()
        {
            if (_isHashedKeys) return _hashedKeys;
            _hashedKeys = StackedData.Keys.ToList();
            _isHashedKeys = true;
            return _hashedKeys;
        }
    }

    public interface IStorageDataStack<TKey, TId, TData>
    {
        IDictionary<TKey, IList<TId>> StackedData { get; }

        IDictionary<TId, TKey> Links { get; }

        IDictionary<TId, TData> Data { get; }

        bool Create(TData data);

        bool Update(TData data);

        bool Delete(TData data);

        IList<TData> GetData(TKey key);

        IList<TData> GetData();

        IList<TKey> GetKeys();
    }
}