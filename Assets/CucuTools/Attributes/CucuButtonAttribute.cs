using System;

namespace CucuTools
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CucuButtonAttribute : Attribute
    {
        public string Name { get; }
        public byte Order { get; }
        public string Group { get; }
        public string Color { get; }
        
        public CucuButtonAttribute(string name = null, byte order = 127, string group = null, string color = null)
        {
            Name = name;
            Order = order;
            Group = group;
            Color = color;
        }
    }
}
