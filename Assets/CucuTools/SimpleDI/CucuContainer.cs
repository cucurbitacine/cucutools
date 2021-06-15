using System;

namespace CucuTools.SimpleDI
{
    public class CucuContainer : IContainer
    {
        public IBinder Bind(Type type)
        {
            throw new NotImplementedException();
        }

        public IBinder Bind<T>()
        {
            throw new NotImplementedException();
        }

        public void Unbind(Type type)
        {
            throw new NotImplementedException();
        }

        public void Unbind<T>()
        {
            throw new NotImplementedException();
        }

        public bool TryResolve(Type type, out object result)
        {
            throw new NotImplementedException();
        }

        public bool TryResolve<T>(out T result)
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type type)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public Guid Guid { get; }
        public string Key { get; }
    }
}