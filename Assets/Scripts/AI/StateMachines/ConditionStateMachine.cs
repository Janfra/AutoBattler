using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TransitionCondition : ScriptableObject
{
    public abstract bool CanTransition();
}

[Serializable]
public struct StateData
{
    [ShowScriptableObjectEditor]
    public State State;
    public StateTransitionData[] Transitions;

    public bool IsValid()
    {
        return State != null;
    }

    public bool HasTransition()
    {
        return Transitions != null && Transitions.Length > 0;
    }
}

[Serializable]
public struct StateTransitionData
{
    public State TransitionState;
    public TransitionCondition[] TransitionConditions;

    public bool IsMeetingTransitionRequirements()
    {
        bool areRequirementsMet = true;

        foreach (TransitionCondition condition in TransitionConditions)
        {
            areRequirementsMet &= condition.CanTransition();
        }

        return areRequirementsMet;
    }
}

public class ConditionStateMachine : BaseStateMachine<StateData>
{
    protected Dictionary<State, StateData> stateRetriever;

    public override void SetState(State targetState)
    {
        if (stateRetriever == null)
        {
            Debug.LogError("State retriever is null");
            return;
        }

        if (!stateRetriever.ContainsKey(targetState))
        {
            Debug.LogError("Attempted to set to state that has not been defined - " + targetState.ToString());
            return;
        }

        currentStateData = stateRetriever[targetState];
    }

    public override bool TryGetStateDataFromState(State targetState, out StateData stateData)
    {
        stateRetriever.TryGetValue(targetState, out stateData);
        return stateData.IsValid();
    }

    public override void RunCurrentState()
    {
        currentStateData.State.RunState();
    }

    public override void AttemptToTransition()
    {
        if (currentStateData.HasTransition())
        {
            StateTransitionData[] transitions = currentStateData.Transitions;
            foreach (StateTransitionData transition in transitions)
            {
                if (transition.IsMeetingTransitionRequirements())
                {
                    SetState(transition.TransitionState);
                    break;
                }
            }
        }
    }

    public override void BakeData()
    {
        if (availableStates.Length <= 0)
        {
            Debug.LogError("Attemped to add states with an empty array");
            return;
        }

        if (stateRetriever == null)
        {
            stateRetriever = new Dictionary<State, StateData>();
            if (stateRetriever == null)
            {
                return;
            }
        }

        foreach (var stateData in availableStates)
        {
            if (!stateData.IsValid())
            {
                continue;
            }

            if (!stateData.State.IsBlackboardValidForState(blackboard))
            {
                Debug.LogError("Blackboard is missing variables for the states to handle");
                continue;
            }

            if (stateRetriever.ContainsKey(stateData.State))
            {
                Debug.LogError("Attempted to add state that is already registered");
                continue;
            }

            if (entryState == null)
            {
                entryState = stateData.State;
            }

            stateData.State.Init(blackboard);
            stateRetriever.Add(stateData.State, stateData);
        }
    }
}
