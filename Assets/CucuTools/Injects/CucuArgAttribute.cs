using System;

namespace CucuTools.Injects
{
    /// <summary>
    /// Attribute for injection field
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class CucuArgAttribute : Attribute
    {
    }
}