using CucuTools.Injects;
using UnityEngine;

namespace Example
{
    public class CucuDI
    {
        public static CucuDI Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = new CucuDI();
                _instance.Setup();
                return _instance;
            }
        }

        private static CucuDI _instance;
        
        private IContainer container;
        
        public IContainer GetContainer()
        {
            return container;
        }
        
        private void Setup()
        {
            container = new CucuContainer();
            
            
            //container.Bind<ILogger>().ToInstance(this);
            container.Bind<ILogger>().ToInstance(Debug.unityLogger);
            //container.Bind<ILogger>().ToSingleton<EmptyLogger>(); // Custom logger for example
        }
    }
}