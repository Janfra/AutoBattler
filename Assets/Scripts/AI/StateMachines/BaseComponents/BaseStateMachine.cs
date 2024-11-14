using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


namespace GameAI
{
    [Serializable]
    public abstract class BaseStateMachine<StateContainer> : MonoBehaviour, IUniqueBlackboardReferencer
    {
        [SerializeField]
        protected State entryState;
        [SerializeField]
        protected StateContainer[] availableStates;
        [SerializeField]
        protected BlackboardBase blackboard;

        protected StateContainer currentStateData;
        protected bool isEnabled = false;

        public virtual void OnReplaceReferences(ReferenceReplacer replacer)
        {
            if (replacer.HasBeenReplaced(this))
            {
                return;
            }

            blackboard.OnReplaceReferences(replacer);
            entryState.OnReplaceReferences(replacer);
            foreach (var state in availableStates)
            {
                if (state is IUniqueBlackboardReferencer referencer)
                {
                    referencer.OnReplaceReferences(replacer);
                }
            }
        }

        private void Start()
        {
            BakeData();
            if (entryState == null)
            {
                return;
            }

            SetState(entryState);
            isEnabled = true;
        }

        private void Update()
        {
            if (isEnabled && currentStateData != null)
            {
                RunCurrentState();
                AttemptToTransition();
            }
        }

        public abstract bool TryGetStateDataFromState(State targetState, out StateContainer stateData);

        public abstract void SetState(State targetState);

        public abstract void RunCurrentState();

        public abstract void AttemptToTransition();

        public virtual void BakeData() { }
    }
}