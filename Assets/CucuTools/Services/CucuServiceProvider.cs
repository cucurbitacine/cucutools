using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CucuTools
{
    [Serializable]
    public class CucuServiceProvider : ICucuServiceProvider
    {
        public static CucuServiceProvider Global { get; }

        private static Dictionary<string, CucuServiceProvider>
            providers = new Dictionary<string, CucuServiceProvider>();

        public Guid guid => _guid;
        public string name => _name;
        private CucuLogger _logger;

        private readonly ConcurrentDictionary<Type, object> _services =
            new ConcurrentDictionary<Type, object>();

        private readonly ConcurrentDictionary<Type, List<Action<object>>> _actions =
            new ConcurrentDictionary<Type, List<Action<object>>>();

        private string _name;
        private Guid _guid;

        public string guidString;
        public string nameString;

        static CucuServiceProvider()
        {
            Global = new CucuServiceProvider("Global");
        }

        public static bool TryGetProvider(string key, out CucuServiceProvider provider)
        {
            return providers.TryGetValue(key, out provider);
        }

        public static bool TryGetProviders(out IEnumerable<CucuServiceProvider> result)
        {
            result = null;

            if (providers == null) return false;

            result = providers.Select(s => s.Value);
            return true;
        }

        public bool TryGetServices(out Dictionary<Type, object> result)
        {
            result = null;

            if (_services == null) return false;

            result = _services.ToDictionary(d => d.Key, d => d.Value);
            return true;
        }

        public CucuServiceProvider(string name)
        {
            this._guid = Guid.NewGuid();
            this._name = name;

            this.nameString = this._name.ToString();
            this.guidString = this._guid.ToString();

            _logger = CucuLogger.Create().SetTag(name + " provider");
            _logger.Log($" initialized");

            providers.Add(name, this);
        }

        public IDictionary<Type, object> GetServices()
        {
            return _services.ToDictionary(d => d.Key, d => d.Value);
        }

        public bool Register<T>(T service) where T : class
        {
            if (_services.ContainsKey(typeof(T)))
            {
                _logger.LogWarning($"\"{typeof(T).FullName}\" already registered");
                return false;
            }

            _services.TryAdd(typeof(T), service);
            _logger.Log($"\"{typeof(T).FullName}\" successfully registered as \"{service.ToString()}\"");

            Internal_InvokeAllActions<T>(service);

            return true;
        }

        public bool Unregister<T>(T service) where T : class
        {
            if (!_services.TryGetValue(typeof(T), out var ser))
            {
                _logger.LogWarning($"\"{typeof(T).FullName}\" wasn't found when try unregistering");
                return false;
            }

            if (ser != service)
            {
                _logger.LogWarning($"You can't unregistering \"{typeof(T).FullName}\", because services don't match");
                return false;
            }

            if (!_services.TryRemove(typeof(T), out var _s))
            {
                _logger.LogWarning(
                    $"Unregistering \"{typeof(T).FullName}\" failed. Service : \"{_s.GetType().FullName}\"");
                return false;
            }

            _logger.Log($"Unregistering \"{typeof(T).FullName}\" successful.");
            return true;
        }

        public bool UnregisterForced<T>() where T : class
        {
            if (!_services.TryGetValue(typeof(T), out var service))
            {
                _logger.LogWarning($"\"{typeof(T).FullName}\" wasn't found when try unregistering");
                return false;
            }

            if (!_services.TryRemove(typeof(T), out var _s))
            {
                _logger.LogError(
                    $"Unregistering forced \"{typeof(T).FullName}\" failed. Service : \"{_s.GetType().FullName}\"");
                return false;
            }

            _logger.Log(
                $"Unregistering forced \"{typeof(T).FullName}\" as \"{service.GetType().FullName}\" successful.");
            return true;
        }

        public bool TryResolve<T>(out T service) where T : class
        {
            service = _services.ContainsKey(typeof(T)) ? (T) _services[typeof(T)] : null;
            _logger.Log(
                $"\"{typeof(T).FullName}\" was{(service == null ? "n't" : "")} resolved{(service != null ? $" as \"{service.GetType().FullName}\"" : "")}");
            return service != null;
        }

        public void Wait<T>(Action<T> action) where T : class
        {
            if (_services.ContainsKey(typeof(T)))
            {
                Internal_InvokeAction<T>(action, _services[typeof(T)]);
                return;
            }

            if (_actions.TryGetValue(typeof(T), out var val))
            {
                val.Add(obj => action.Invoke(obj as T));
            }
            else
            {
                _actions.TryAdd(typeof(T), new List<Action<object>> {obj => action.Invoke(obj as T)});
            }

            _logger.Log($"Waiting \"{typeof(T).FullName}\"");
        }

        private void Internal_InvokeAllActions<T>(T obj) where T : class
        {
            if (!_actions.ContainsKey(typeof(T))) return;

            foreach (var action in _actions[typeof(T)])
                Internal_InvokeAction<T>(action, obj);

            _actions[typeof(T)].Clear();
        }

        private void Internal_InvokeAction<T>(Action<T> action, object obj) where T : class
        {
            try
            {
                action.Invoke(obj as T);

                _logger.Log($"\"{typeof(T).FullName}\" was resolved as \"{obj.GetType().FullName}\"");
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
            }
        }
    }

    public interface ICucuServiceProvider
    {
        Guid guid { get; }
        string name { get; }

        bool Register<T>(T service) where T : class;
        bool Unregister<T>(T service) where T : class;
        bool UnregisterForced<T>() where T : class;
        bool TryResolve<T>(out T service) where T : class;
        void Wait<T>(Action<T> action) where T : class;
    }
}