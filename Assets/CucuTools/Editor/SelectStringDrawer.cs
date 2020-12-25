using UnityEditor;

namespace CucuTools.Editor
{
    [CustomPropertyDrawer(typeof(SelectStringAttribute))]
    public sealed class SelectStringDrawer : SelectStringDrawerBase<SelectStringAttribute>
    {
    }
}