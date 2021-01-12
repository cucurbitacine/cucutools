using UnityEngine;

namespace CucuTools
{
    public class CucuSelectAttribute : PropertyAttribute
    {
        protected string[] strings;
        
        public CucuSelectAttribute(params string[] strings)
        {
            this.strings = strings;
        }

        public virtual string[] GetStrings()
        {
            return strings;
        }
    }
}