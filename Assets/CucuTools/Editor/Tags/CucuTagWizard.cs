using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor.Tags
{
    public class CucuTagWizard : ScriptableWizard
    {
        public const string MenuTagsRoot = Cucu.MenuRoot + "Tags/";

        [MenuItem(Cucu.MenuCreateRoot + nameof(CucuTag))]
        [MenuItem(MenuTagsRoot + "Create tag")]
        public static void CreateCucuTag()
        {
            var tag = new GameObject("CucuTag").AddComponent<CucuTag>();
            Selection.objects = new[] {tag.gameObject};
        }
    }
}