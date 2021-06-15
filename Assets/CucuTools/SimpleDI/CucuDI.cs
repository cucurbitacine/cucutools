using CucuTools.Buttons;
using UnityEngine;

namespace CucuTools.SimpleDI
{
    public class CucuDI : CucuBehaviour, ILog
    {
        private IContainer container => _container ?? (_container = new CucuContainer());
        private IContainer _container;

        [CucuButton()]
        private void NewInstance()
        {
            container.Bind<ILog>().To<CucuDI>();

            container.Resolve<ILog>().Log("Hello world");
        }

        [CucuButton()]
        private void Instance()
        {
            container.Bind<ILog>().ToInstance(this);

            container.Resolve<ILog>().Log("Hello world");
        }

        [CucuButton()]
        private void Singleton()
        {
            container.Bind<ILog>().ToSingleton<CucuDI>();

            container.Resolve<ILog>().Log("Hello world");
        }

        public void Log(string msg)
        {
            Debug.Log($"[{gameObject.name}] {msg}");
        }
    }

    public interface ILog
    {
        void Log(string msg);
    }
}