using UnityEditor;

namespace CucuTools.Editor.Attributes
{
    [CustomPropertyDrawer(typeof(CucuSelectAttribute))]
    public sealed class CucuSelectDrawer : CucuSelectDrawerBase<CucuSelectAttribute>
    {
    }
}