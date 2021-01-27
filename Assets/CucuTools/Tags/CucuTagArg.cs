using System;

namespace CucuTools
{
    [Serializable]
    public struct CucuTagArg
    {
        public string key;
        public string value;

        public CucuTagArg(string key, string value)
        {
            this.key = key;
            this.value = value;
        }
        
        public static bool operator ==(CucuTagArg arg1, CucuTagArg arg2)
        {
            return arg1.key.Equals(arg2.key) && arg1.value.Equals(arg2.value);
        }

        public static bool operator !=(CucuTagArg arg1, CucuTagArg arg2)
        {
            return !(arg1 == arg2);
        }

        public override bool Equals(object obj)
        {
            return obj is CucuTagArg arg && this == arg;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}