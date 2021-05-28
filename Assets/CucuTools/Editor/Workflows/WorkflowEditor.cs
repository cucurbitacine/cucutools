using CucuTools.Workflows;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor.Workflows
{
    public static class WorkflowEditor
    {
        public const string AddComponent = Cucu.AddComponent;
        
        [MenuItem(Cucu.CreateGameObject + WorkflowBehaviour.ObjectName)]
        public static void CreateWorkflow()
        {
            Selection.objects = new Object[]
            {
                new GameObject(WorkflowBehaviour.ObjectName).AddComponent<WorkflowBehaviour>().gameObject
            };
        }
    }
}