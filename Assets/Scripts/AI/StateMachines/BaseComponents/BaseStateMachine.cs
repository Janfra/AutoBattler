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

        private bool isTransitionLocked;

        [SerializeField]
        protected bool isDebugActive;
        [SerializeField]
        protected TextMesh debugText;

        public virtual void OnReplaceReferences(BlackboardReferenceReplacer replacer)
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

        private void Awake()
        {
            if (!blackboard)
            {
                throw new NullReferenceException();
            }

            blackboard.OnPopulated += OnBlackboardReady;
        }

        private void Update()
        {
            if (isEnabled && currentStateData != null)
            {
                RunCurrentState();

                if (!isTransitionLocked)
                {
                    AttemptToTransition();
                }

                if (isDebugActive)
                {
                    debugText.text = GetCurrentStateName();
                }
            }
        }

        public void SetLockTransition(bool isLocked)
        {
            isTransitionLocked = isLocked;
        }

        public abstract bool TryGetStateDataFromState(State targetState, out StateContainer stateData);

        public abstract void SetState(State targetState);

        public abstract void RunCurrentState();

        public abstract void AttemptToTransition();

        protected abstract string GetCurrentStateName();

        public virtual void BakeData() { }

        protected void OnBlackboardReady()
        {
            BakeData();
            if (entryState == null)
            {
                return;
            }

            SetState(entryState);
            isEnabled = true;

            blackboard.OnPopulated -= OnBlackboardReady;
        }
    }
}