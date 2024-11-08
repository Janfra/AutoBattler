using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI
{
    public class DesireStateMachine : BaseStateMachine<StateDesire>
    {
        protected Dictionary<State, StateDesire> stateRetriever = new Dictionary<State, StateDesire>();

        public override void AttemptToTransition()
        {
            float highestDesire = 0.0f;
            StateDesire transitionState = null;
            foreach (StateDesire stateDesire in availableStates)
            {
                float currentDesire = stateDesire.GetDesireValue();
                if (currentDesire > highestDesire)
                {
                    highestDesire = currentDesire;
                    transitionState = stateDesire;
                }
            }

            if (transitionState != null && !currentStateData.IsSameTarget(transitionState))
            {
                SetState(transitionState.Target);
            }
        }

        public override void RunCurrentState()
        {
            currentStateData.Target.RunState();
        }

        public override void SetState(State targetState)
        {
            if (stateRetriever.ContainsKey(targetState))
            {
                if (currentStateData != null)
                {
                    currentStateData.Target.StateExited();
                }

                currentStateData = stateRetriever[targetState];
                currentStateData.Target.StateEntered();
            }
        }

        public override bool TryGetStateDataFromState(State targetState, out StateDesire stateData)
        {
            if (stateRetriever.ContainsKey(targetState))
            {
                stateData = stateRetriever[targetState];
                return true;
            }

            stateData = null;
            return false;
        }

        public override void BakeData()
        {
            if (availableStates == null || availableStates.Length <= 0)
            {
                Debug.LogError("Attemped to add states with an empty array");
                return;
            }

            if (stateRetriever == null)
            {
                stateRetriever = new Dictionary<State, StateDesire>();
                if (stateRetriever == null)
                {
                    return;
                }
            }

            foreach (StateDesire desire in availableStates)
            {
                if (!desire || !desire.IsValid())
                {
                    continue;
                }

                if (!desire.IsBlackboardValidForState(blackboard))
                {
                    Debug.LogError("Blackboard is missing variables for the states to handle");
                    continue;
                }

                if (stateRetriever.ContainsKey(desire.Target))
                {
                    Debug.LogError("Attempted to add state that is already registered");
                    continue;
                }

                if (entryState == null)
                {
                    entryState = desire.Target;
                }

                desire.InitReferences(blackboard);
                desire.Target.Init(blackboard);
                stateRetriever.Add(desire.Target, desire);
            }
        }
    }
}