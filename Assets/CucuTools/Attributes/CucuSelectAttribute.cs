using UnityEngine;

namespace CucuTools.Attributes
{
    /// <summary>
    ///Attribute to display custom popup with the specified strings
    /// </summary>
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