using System;
using System.Collections.Generic;
using UnityEngine;

namespace CucuTools.SimpleDI
{
    public class CucuContainer : IContainer
    {
        private Dictionary<Type, IBinder> Map;

        public CucuContainer(string key = null)
        {
            Guid = Guid.NewGuid();
            Key = key ?? Guid.ToString();

            Map = new Dictionary<Type, IBinder>();
        }
        
        public IBinder Bind(Type type)
        {
            Map[type] = new CucuBinder();
            return Map[type];
        }

        public IBinder Bind<T>()
        {
            return Bind(typeof(T));
        }

        public void Unbind(Type type)
        {
            Map.Remove(type);
        }

        public void Unbind<T>()
        {
            Unbind(typeof(T));
        }

        public bool TryResolve(Type type, out object result)
        {
            if (Map.TryGetValue(type, out var bind))
            {
                result = bind.Condition.Get();
                return true;
            }

            result = default;
            return false;
        }

        public bool TryResolve<T>(out T result)
        {
            if (Map.TryGetValue(typeof(T), out var bind))
            {
                result = bind.Condition.Get<T>();
                return true;
            }

            result = default;
            return false;
        }

        public object Resolve(Type type)
        {
            return TryResolve(type, out var result) ? result : default;
        }

        public T Resolve<T>()
        {
            return TryResolve<T>(out var result) ? result : default;
        }

        public Guid Guid { get; }
        public string Key { get; }
    }

    public class CucuBinder : IBinder
    {
        public IBindCondition Condition { get; protected set; }
        
        public void To(Type type)
        {
            Condition = new CucuBindCreate(type);
        }

        public void To<T>()
        {
            To(typeof(T));
        }

        public void ToSingleton(Type type)
        {
            ToInstance(type, ObjectFactory.Instance.Create(type));
        }

        public void ToSingleton<T>()
        {
            ToSingleton(typeof(T));
        }

        public void ToInstance(Type type, object instance)
        {
            Condition = new CucuBindInstance(type, instance);
        }

        public void ToInstance<T>(T instance)
        {
            ToInstance(typeof(T), instance);
        }
    }

    public abstract class CucuBindBase : IBindCondition
    {
        public Type TargetType { get; }
        
        public CucuBindBase(Type targetType)
        {
            TargetType = targetType;
        }

        public abstract object Get();

        public T Get<T>() => (T) Get();
    }

    public class CucuBindCreate : CucuBindBase
    {
        public CucuBindCreate(Type targetType) : base(targetType)
        {
        }

        public override object Get()
        {
            return ObjectFactory.Instance.Create(TargetType);
        }
    }

    public class CucuBindInstance : CucuBindBase
    {
        private object _instance;

        public CucuBindInstance(Type targetType, object instance) : base(targetType)
        {
            _instance = instance;
        }

        public override object Get()
        {
            return _instance;
        }
    }

    public interface ICanCreateObject
    {
        object Create(Type type);
        T Create<T>();
    }

    public class ObjectFactory : ICanCreateObject
    {
        public static ObjectFactory Instance
        {
            get
            {
                if (_instance == null) _instance = new ObjectFactory();

                return _instance;
            }
        }

        private static ObjectFactory _instance;
        
        public object Create(Type type)
        {
            if (type.IsSubclassOf(typeof(Component)))
            {
                return new GameObject(type.Name).AddComponent(type);
            }
            else
            {
                return Activator.CreateInstance(type);
            }
        }

        public T Create<T>()
        {
            return (T) Create(typeof(T));
        }
    }
}