using System;

namespace CucuTools.Attributes
{
    /// <summary>
    /// Cucu button attribtue.
    /// Put it before some method for creating button into unity inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CucuButtonAttribute : Attribute
    {
        public string Name { get; }
        public byte Order { get; }
        public string Group { get; }
        public string ColorHex { get; }
        
        public CucuButtonAttribute(string name = null, byte order = 127, string group = null, string colorHex = null)
        {
            Name = name;
            Order = order;
            Group = group;
            ColorHex = colorHex ?? "888888";
        }
    }
}
