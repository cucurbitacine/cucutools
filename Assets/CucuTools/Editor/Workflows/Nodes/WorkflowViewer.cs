using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CucuTools.Editor.Workflows.Nodes
{
    public class WorkflowViewer : EditorWindow
    {
        [MenuItem(Cucu.Tools + "Workflow Viewer")]
        private static void ShowWindow()
        {
            var window = GetWindow<WorkflowViewer>();
            window.titleContent = new GUIContent("Workflow Viewer");
            window.Show();
        }

        private List<NodeBase> nodes;
        private GUIStyle stateNodeStyle;
        private GUIStyle workflowNodeStyle;
        
        private void DrawNodes()
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Draw();
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;
            }
        }
        
        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e);
 
                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }
        
        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Create State"), false, () => OnClickAddWorkflowNode(mousePosition)); 
            genericMenu.AddItem(new GUIContent("Create Workflow"), false, () => OnClickAddStateNode(mousePosition)); 
            genericMenu.ShowAsContext();
        }
 
        private void OnClickAddStateNode(Vector2 mousePosition)
        {
            if (nodes == null)
            {
                nodes = new List<NodeBase>();
            }
 
            nodes.Add(new StateNode(mousePosition, 200, 50, stateNodeStyle));
        }
        
        private void OnClickAddWorkflowNode(Vector2 mousePosition)
        {
            if (nodes == null)
            {
                nodes = new List<NodeBase>();
            }
 
            nodes.Add(new WorkflowNode(mousePosition, 200, 50, stateNodeStyle));
        }
        
        private void OnGUI()
        {
            DrawNodes();
            
            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        private void OnEnable()
        {
            stateNodeStyle = new GUIStyle();
            stateNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            stateNodeStyle.border = new RectOffset(12, 12, 12, 12);
            
            workflowNodeStyle = new GUIStyle();
            workflowNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            workflowNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }
    }
}