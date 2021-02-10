using System;

namespace CucuTools
{
    [Serializable]
    public struct CucuArg
    {
        public string key;
        public string value;

        public CucuArg(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
        
        public static bool operator ==(CucuArg arg1, CucuArg arg2)
        {
            return arg1.key.Equals(arg2.key) && arg1.value.Equals(arg2.value);
        }

        public static bool operator !=(CucuArg arg1, CucuArg arg2)
        {
            return !(arg1 == arg2);
        }

        public override bool Equals(object obj)
        {
            return obj is CucuArg arg && this == arg;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}