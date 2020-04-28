using System;

namespace CucuTools
{
    [Serializable]
    public struct CucuTagArg
    {
        public string key;
        public string value;

        public override bool Equals(object obj)
        {
            if (!(obj is CucuTagArg arg)) return false;
            return arg.key.Equals(key) && arg.value.Equals(value);
        }
    }
}