using System;
using System.Collections.Generic;
using System.Linq;

namespace CucuTools
{
    [Serializable]
    public class CucuArgumentManager
    {
        public static CucuArgumentManager Instance { get; }

        public string Name { get; }

        private List<CucuArg> Args => _args ?? (_args = new List<CucuArg>());
        private List<CucuArg> _args;
        
        static CucuArgumentManager()
        {
            Instance = new CucuArgumentManager("static");
        }

        public CucuArgumentManager(string name)
        {
            Name = name;
        }
        
        public void SetArguments(params object[] args)
        {
            Clear();
            AddArguments(args);
        }

        public void AddArguments(params object[] args)
        {
            Args.AddRange(args.Where(a => a != null).OfType<CucuArg>());
        }

        public void Clear()
        {
            Args.Clear();
        }
        
        public T[] GetArgs<T>()
        {
            return Args.OfType<T>().ToArray();
        }

        public CucuArg[] GetArgs(Type argType = null)
        {
            if (argType == null) return Args.ToArray();

            return Args.Where(a => a.GetType() == argType).ToArray();
        }

        public bool TryGetArgs<T>(out T arg) where T : CucuArg
        {
            return (arg = GetArgs<T>().FirstOrDefault()) != null;
        }

        public bool TryGetArgs(Type argType, out CucuArg arg)
        {
            return (arg = GetArgs(argType).FirstOrDefault()) != null;
        }
    }
}