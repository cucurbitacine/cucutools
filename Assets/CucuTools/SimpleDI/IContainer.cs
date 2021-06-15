using System;

namespace CucuTools.SimpleDI
{
    public interface IContainer : ICanBinding, ICanResolving
    {
        public Guid Guid { get; }
        public string Key { get; }
    }

    public interface ICanBinding
    {
        IBinder Bind(Type type);
        IBinder Bind<T>();
        
        void Unbind(Type type);
        void Unbind<T>();
    }

    public interface ICanResolving
    {
        bool TryResolve(Type type, out object result);
        bool TryResolve<T>(out T result);
        
        object Resolve(Type type);
        T Resolve<T>();
    }
    
    public interface IBinder
    {
        IBindCondition Condition { get; }
        
        void To(Type type);
        void To<T>();

        void ToSingleton(Type type);
        void ToSingleton<T>();

        void ToInstance(Type type, object instance);
        void ToInstance<T>(T instance);
    }

    public interface IBindCondition
    {
        object Get();
        T Get<T>();
    }
}