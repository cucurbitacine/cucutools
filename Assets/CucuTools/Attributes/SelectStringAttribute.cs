using UnityEngine;

namespace CucuTools
{
    public class SelectStringAttribute : PropertyAttribute
    {
        protected string[] strings;
        
        public SelectStringAttribute(params string[] strings)
        {
            this.strings = strings;
        }

        public virtual string[] GetStrings()
        {
            return strings;
        }
    }
}