using System;
using System.Collections.Generic;
using CucuTools.Attributes;
using CucuTools.Statemachines.Core;
using UnityEngine;

public class StateMachineLineBuilder : MonoBehaviour
{
    public Transform parent;
    public LineRenderer linePrefab;
    public StateMachineEntity mainState;

    private StateEntity[] states;
    
    private Dictionary<TransitionEntity, LineRenderer> lines = new Dictionary<TransitionEntity, LineRenderer>();
    private Dictionary<StateMachineEntity, LineRenderer> helpLines = new Dictionary<StateMachineEntity, LineRenderer>();
    
    [CucuButton]
    public void Build()
    {
        foreach (var line in lines)
        {
            try
            {
                if (Application.isPlaying)
                {
                    Destroy(line.Value.gameObject);
                }
                else
                {
                    DestroyImmediate(line.Value.gameObject);
                }
            }
            catch
            {
                //
            }
        }
        
        
        lines.Clear();
        
        foreach (var line in helpLines)
        {
            try
            {
                if (Application.isPlaying)
                {
                    Destroy(line.Value.gameObject);
                }
                else
                {
                    DestroyImmediate(line.Value.gameObject);
                }
            }
            catch
            {
                //
            }
        }
        
        helpLines.Clear();
        
        states = GetComponentsInChildren<StateEntity>();

        foreach (var state in states)
        {
            foreach (var transition in state.Transitions)
            {
                var line = Instantiate(linePrefab, parent, false);
                line.useWorldSpace = true;
                line.SetPositions(new[] {state.transform.position, transition.Target.transform.position});

                if (transition.Target is StateMachineEntity machine)
                {
                    if (!helpLines.ContainsKey(machine))
                    {
                        var helpLine = Instantiate(linePrefab, parent, false);
                        helpLine.useWorldSpace = true;
                        helpLine.SetPositions(new[] {machine.transform.position, machine.Current.transform.position});

                        helpLines.Add(machine, helpLine);
                    }
                }
                
                lines.Add(transition, line);
            }
        }
    }

    private void Start()
    {
        Build();
    }
    
    private void Update()
    {
        if (!mainState.IsPlaying) return;
        
        foreach (var state in states)
        {
            foreach (var transition in state.Transitions)
            {
                if (lines.TryGetValue(transition, out var line))
                {
                    line.enabled = (state is StateMachineEntity sme)
                        ? state.IsPlaying && sme.Current.IsLast
                        : state.IsPlaying;
                    line.startWidth = 0.4f;
                    line.endWidth = 0.05f;
                }
            }
        }
    }
}
