using System;

namespace CucuTools.Attributes
{
    /// <summary>
    /// Attribute for injection field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class CucuArgAttribute : Attribute
    {
    }
}