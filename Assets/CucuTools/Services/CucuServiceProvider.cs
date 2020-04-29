using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CucuTools
{
    public class CucuServiceProvider
    {
        public static CucuServiceProvider Global { get; }

        public Guid guid { get; }
        public string name { get; }

        private CucuLogger _logger;

        private readonly ConcurrentDictionary<Type, object> _services;
        private readonly ConcurrentDictionary<Type, List<Action<object>>> _actions;

        static CucuServiceProvider()
        {
            Global = new CucuServiceProvider("Global");
        }

        public CucuServiceProvider(string name)
        {
            this.guid = Guid.NewGuid();
            this.name = name;

            this._services = new ConcurrentDictionary<Type, object>();
            this._actions = new ConcurrentDictionary<Type, List<Action<object>>>();

            _logger = CucuLogger.Create().SetTag(name + " provider");
            _logger.Log($" initialized");
        }

        public IDictionary<Type, object> GetServices()
        {
            return _services.ToDictionary(d => d.Key, d => d.Value);
        }

        public bool Register<T>(T service) where T : class
        {
            if (_services.ContainsKey(typeof(T)))
            {
                _logger.Log($"\"{typeof(T).FullName}\" already registered", logType: LogType.Warning);
                return false;
            }

            _services.TryAdd(typeof(T), service);
            _logger.Log($"\"{typeof(T).FullName}\" successfully registered as \"{service.ToString()}\"");

            Internal_InvokeAllActions<T>(service);

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
                _logger.Log($"{e.Message}", logType: LogType.Error);
            }
        }
    }
}